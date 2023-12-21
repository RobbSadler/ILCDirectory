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
    // The SearchForPerson method could be implemented in SQL, but I chose to implement it in C#
    // because I wanted to show the steps involved in the search, and the data set in the database 
    // is small enough that it can be done in memory. There are less than 10000 persons in the database.
    public async Task<(IList<Person>, IList<Address>)> SearchForPersonOrAddress(IConfiguration config, string searchString)
    {
        ILCDirectoryRepository repo = new ILCDirectoryRepository();
        List<SearchToken> searchTokens = new List<SearchToken>();
        // use a regex to tokenize the string into words
        var regex = new Regex(@"\w+");
        var tokens = regex.Matches(searchString.ToLower()); // all tokens are lowercase

        // eliminate duplicate tokens
        var distinctTokens = tokens.Cast<Match>().Select(m => m.Value).Distinct();

        // find all the tokens in SearchTokens table in the database
        using var conn = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var sql = "SELECT * FROM SearchToken WHERE Token IN ('" + string.Join("','", distinctTokens) + "')";
        var rows = await cmd.SetCommandText(sql)
            .ExecuteToListAsync<SearchToken>();

        List<Person> persons = new();
        List<Address> addresses = new();

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

            if (addressIds.Count > 0)
                addresses = (List<Address>) await repo.GetRowsByIdsAsync<Address>(config, addressIds, "Address");

            // now get the personIds from the SearchTokenPerson table using the same tokens in distinctTokens
            using var conn3 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
            var cmd4 = Sqlocity.GetDatabaseCommand(conn3);
            var sql4 = "SELECT PersonId FROM SearchTokenPerson WHERE SearchTokenId IN (" +
                string.Join(",", rows.Select(r => r.SearchTokenId).Distinct().ToArray()) + ") " +
                "GROUP BY PersonId " + // take only the PersonIds where every token is matched
                "HAVING COUNT(*) = " + distinctTokens.Count(); // the count of tokens must equal the number of tokens searched for
            var personIds = await cmd4.SetCommandText(sql4)
                .ExecuteToListAsync<int>();

            if (personIds.Count > 0)
                persons = (List<Person>) await repo.GetRowsByIdsAsync<Person>(config, personIds, "Person");
        }

        return (persons, addresses);
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
            string[] toParse = { row.FirstName, row.LastName, row.NickName, row.MaidenName, row.FieldOfService, row.MiddleName, row.Position, row.WoCode };

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
