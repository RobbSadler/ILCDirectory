using ILCDirectory.Data.Enums;
using ILCDirectory.Data.Helpers;
using ILCDirectory.Data.Models;
using ILCDirectory.Data.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using SqlocityNetCore;
using System;
using System.Data.Common;
using System.Diagnostics;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constants = ILCDirectory.Data.Helpers.Constants;

namespace ILCDirectory.Data.Repositories;

public class TokenizeAndSearchRepository : ITokenizeAndSearchRepository
{
    // search the tokens in the SearchToken table and return the PersonIds and AddressIds that are found
    public async Task<IList<Person>> SearchForPersonOrAddress(IConfiguration config, string searchString, bool searchPartialWords, 
        bool includeChildren, bool localOnly)
    {
        ILCDirectoryRepository repo = new ILCDirectoryRepository();
        List<SearchToken> searchTokens = new List<SearchToken>();
        // use a regex to tokenize the string into words
        var regex = new Regex(@"\w+");
        var tokens = regex.Matches(searchString.ToLower()); // all tokens are lowercase

        // eliminate duplicate tokens
        var distinctTokens = tokens.Cast<Match>().Select(m => m.Value).Distinct();

        var rows = new List<SearchToken>();
        if (searchPartialWords)
        {
            // make an alternative to the below SQL statement that gathers all tokens that match via a LIKE %searchterm% statement
            // this will allow for partial matches. It would be prohibitively expensive (i.e. take too long) to do this for a larger database
            foreach (var token in distinctTokens)
            {
                using var connPartial = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmdPartial = Sqlocity.GetDatabaseCommand(connPartial);
                var sqlPartial = "SELECT * FROM SearchToken WHERE Token LIKE '%" + token + "%'";
                var rowsPartial = await cmdPartial.SetCommandText(sqlPartial)
                    .ExecuteToListAsync<SearchToken>();
                rows.AddRange(rowsPartial);
            }
        }
        else
        {
            // find all the tokens in SearchTokens table in the database
            using var conn = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
            var cmd = Sqlocity.GetDatabaseCommand(conn);
            var sql = "SELECT * FROM SearchToken WHERE Token IN ('" + string.Join("','", distinctTokens) + "')";
            rows = await cmd.SetCommandText(sql)
                    .ExecuteToListAsync<SearchToken>();
        }

        List<Person> persons = new();

        if (rows.Count > 0)
        {
            // find all the tokens in SearchTokenAddress table in the database
            // where every token is matched with an AddressId
            using var conn2 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
            var cmd2 = Sqlocity.GetDatabaseCommand(conn2);
            var sql2 = "SELECT AddressId FROM SearchTokenAddress WHERE SearchTokenId IN (" +
                string.Join(",", rows.Select(r => r.SearchTokenId).Distinct().ToArray()) + ") " +
                "GROUP BY AddressId " + // take only the AddressIds where every token is matched
                "HAVING COUNT(*) = " + distinctTokens.Count(); // the count of tokens must equal the number of tokens searched for
            var addressIds = await cmd2.SetCommandText(sql2)
                .ExecuteToListAsync<int>();

            List<int> personIdsFromAddressSearch = new();
            if (addressIds.Count > 0)
            {
                // get personIds from the Address table via the HouseholdAddress table and the PersonHousehold table
                using var conn5 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmd5 = Sqlocity.GetDatabaseCommand(conn5);
                var sql5 = "SELECT PersonId FROM PersonHousehold WHERE HouseholdId IN (" +
                    "SELECT HouseholdId FROM HouseholdAddress WHERE AddressId IN (" +
                    string.Join(",", addressIds) + "))";
                personIdsFromAddressSearch = await cmd5.SetCommandText(sql5)
                    .ExecuteToListAsync<int>();
            }

            // now get the personIds from the SearchTokenPerson table using the same tokens in distinctTokens
            using var conn3 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
            var cmd4 = Sqlocity.GetDatabaseCommand(conn3);
            var sql4 = "SELECT PersonId FROM SearchTokenPerson WHERE SearchTokenId IN (" +
                string.Join(",", rows.Select(r => r.SearchTokenId).Distinct().ToArray()) + ") " +
                "GROUP BY PersonId " + // take only the PersonIds where every token is matched
                "HAVING COUNT(*) = " + distinctTokens.Count(); // the count of tokens must equal the number of tokens searched for
            var personIds = await cmd4.SetCommandText(sql4)
                .ExecuteToListAsync<int>();

            personIds.AddRange(personIdsFromAddressSearch);

            // get the persons from the Person table, and filter for the switches that have been set
            // if LocalSearch is true, then only consider zipcodes that start with 75 or 76
            // if IncludeChildren is false, then only consider persons with a birthdate before Today minus 18 years
            if (personIds.Count > 0)
            {
                var sqlFinal = @"SELECT * FROM Person ";
                if (localOnly)
                {
                    sqlFinal += "INNER JOIN PersonHousehold ON Person.PersonId = PersonHousehold.PersonId" +
                        "INNER JOIN HouseholdAddress ON PersonHousehold.HouseholdId = HouseholdAddress.HouseholdId" +
                        "INNER JOIN Address ON HouseholdAddress.AddressId = Address.AddressId";
                }
                sqlFinal += "WHERE PersonId IN (" + string.Join(",", personIds) + @") ";
                if (localOnly)
                {
                    sqlFinal += "AND Address.PostalCode LIKE '75%' OR Address.PostalCode LIKE '76%' ";
                }

                if (!includeChildren)
                {
                    sqlFinal += "AND DateOfBirth < DATEADD(year, -18, GETDATE()) ";
                }
                var connFinal = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmdFinal = Sqlocity.GetDatabaseCommand(connFinal);
                persons = await cmdFinal.SetCommandText(sqlFinal)
                    .ExecuteToListAsync<Person>();
            }
        }

        return persons;
    }

    public async Task PopulateSearchTokenTables(IConfiguration config)
    {
        using var conn = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var sql = "SELECT * FROM Address";
        var addressRows = await cmd.SetCommandText(sql)
            .ExecuteToListAsync<Address>();

        foreach (var row in addressRows)
        {
            // for each address, tokenize the address and insert the tokens into the SearchToken table
            // use SearchToken table to put only one distinct value for each token into the table
            // then insert the SearchTokenId and AddressId into the SearchTokenAddress table
            var regex = new Regex(@"\w+");
            // do for AddressLine1, AddressLine2, City, State, Zip, and Country
            string[] toParse = { row.AddressLine1, row.AddressLine2, row.City, row.StateProvince, row.PostalCode } ;

            List<string> tokens = new ();
            foreach (var item in toParse)
            {
                if (item == null) continue;
                tokens.AddRange(regex.Matches(item.ToLower()).Select(match => match.Value).ToList());
            }

            // get unique tokens
            var distinctTokens = tokens.Distinct();
            foreach (var token in distinctTokens)
            {
                sql = @"
                    DECLARE @id INT
                    SELECT @id = SearchTokenId FROM SearchToken WHERE Token = @token
                    IF (@id IS NULL)
                    BEGIN
	                    INSERT INTO SearchToken (Token) VALUES (@token)
	                    SELECT @id = @@IDENTITY
                    END

                    INSERT INTO SearchTokenAddress (SearchTokenId, AddressId)
                    VALUES (@id, @addressId)

                    SELECT @@IDENTITY
                    ";


                using var conn2 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmd2 = Sqlocity.GetDatabaseCommand(conn2);

                cmd2.AppendCommandText(sql);
                cmd2.AddParameter("@token", token);
                cmd2.AddParameter("@addressId", row.AddressId);
                var id = await cmd2.ExecuteScalarAsync<int>();
            }
        }

        using var conn3 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmd3 = Sqlocity.GetDatabaseCommand(conn3);
        sql = "SELECT * FROM Person";
        var personRows = await cmd3.SetCommandText(sql)
            .ExecuteToListAsync<Person>();

        foreach (var row in personRows)
        {
            // for each person, tokenize the person and insert the tokens into the SearchToken table
            // use SearchToken table to put only one distinct value for each token into the table
            // then insert the SearchTokenId and PersonId into the SearchTokenPerson table
            var regex = new Regex(@"\w+");
            // do for PersonLine1, PersonLine2, City, State, Zip, and Country
            string[] toParse = { row.FirstName, row.LastName, row.NickName, row.MaidenName, row.LanguagesSpoken, row.ClassificationCode, row.FieldOfService, row.MiddleName, row.Position, row.WoCode };

            List<string> tokens = new();
            foreach (var item in toParse)
            {
                if (item == null) continue;
                tokens.AddRange(regex.Matches(item.ToLower()).Select(match => match.Value).ToList());
            }

            // get unique tokens
            var distinctTokens = tokens.Distinct();
            foreach (var token in distinctTokens)
            {
                sql = @"
                    DECLARE @id INT
                    SELECT @id = SearchTokenId FROM SearchToken WHERE Token = @token
                    IF (@id IS NULL)
                    BEGIN
	                    INSERT INTO SearchToken (Token) VALUES (@token)
	                    SELECT @id = @@IDENTITY
                    END

                    INSERT INTO SearchTokenPerson (SearchTokenId, PersonId)
                    VALUES (@id, @personId)

                    SELECT @@IDENTITY
                    ";

                using var conn4 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmd4 = Sqlocity.GetDatabaseCommand(conn4);

                var searchTokenPersonId = await cmd4.SetCommandText(sql)
                    .AddParameter("@token", token)
                    .AddParameter("@personId", (int)row.PersonId)
                    .ExecuteScalarAsync<int>();
            }
        }
    }
}
