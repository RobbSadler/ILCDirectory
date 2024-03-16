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
    private readonly IConfiguration _config;
    public TokenizeAndSearchRepository(IConfiguration config)
    {
        _config = config;
    }

    // search the tokens in the SearchToken table and return the Persons and Address rows that are found / related
    public async Task<(IList<Person>, IList<HouseholdAddress>)> SearchForPersonOrAddress(IConfiguration config, string searchString, bool searchPartialWords, 
        bool includeChildren, bool localOnly)
    {
        searchString = (searchString ?? "").Trim();
        List<SearchToken> searchTokens = new List<SearchToken>();
        // use a regex to tokenize the string into words
        var regex = new Regex(@"\w+");
        var tokens = regex.Matches(searchString.ToLower()); // all tokens are lowercase

        // eliminate duplicate tokens
        var distinctTokens = tokens.Cast<Match>().Select(m => m.Value).Distinct();

        var rows = new List<SearchToken>();
        List<Person> persons = new();
        List<int> personIds = new();

        if (searchString.Length > 0)
        {
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
                var personIdFromPersonSearchs = await cmd4.SetCommandText(sql4)
                    .ExecuteToListAsync<int>();

                personIds.AddRange(personIdFromPersonSearchs);
                personIds.AddRange(personIdsFromAddressSearch);
            }
        }

        if (searchString.Length > 0 && personIds.Count == 0) // search returned no results
            return (persons, new List<HouseholdAddress>());

        // get the persons from the Person table, and filter for the switches that have been set
        // if LocalSearch is true, then only consider zipcodes that start with 75 or 76
        // if IncludeChildren is false, then only consider persons with a birthdate before Today minus 18 years
        var sqlPersons = @"SELECT * FROM Person ";
        if (localOnly)
        {
            sqlPersons += " INNER JOIN PersonHousehold ON Person.PersonId = PersonHousehold.PersonId " +
                " INNER JOIN HouseholdAddress ON PersonHousehold.HouseholdId = HouseholdAddress.HouseholdId " +
                " INNER JOIN Address ON HouseholdAddress.AddressId = Address.AddressId ";
        }
        
        if (personIds.Count > 0 && searchString.Length > 0)
            sqlPersons += " WHERE Person.PersonId IN (" + string.Join(",", personIds) + @") ";
        else 
            sqlPersons += " WHERE 1 = 1 ";

        if (localOnly)
            sqlPersons += " AND Address.CountryISO3 = 'USA' AND (Address.PostalCode LIKE '75%' OR Address.PostalCode LIKE '76%') ";

        if (!includeChildren)
        {
            sqlPersons += " AND DateOfBirth < DATEADD(year, -18, GETDATE()) ";
        }
        var connPersons = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmdPersons = Sqlocity.GetDatabaseCommand(connPersons);
        persons = await cmdPersons.SetCommandText(sqlPersons)
            .ExecuteToListAsync<Person>();

        IList<Address> addresses = new List<Address>();
        IList<InternalAddress> internalAddresses = new List<InternalAddress>();
        IList<PersonHousehold> personHouseholds = new List<PersonHousehold>();
        IList<HouseholdAddress> householdAddresses = new List<HouseholdAddress>();

        // get all of the personhouseholds and householdaddresses for the persons
        if (persons.Count > 0)
        {
            personIds = persons.Select(p => (int)p.PersonId!).ToList<int>();
            if (personIds.Count > 0)
            {
                var connPersonHousehold = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmdPersonHousehold = Sqlocity.GetDatabaseCommand(connPersonHousehold);
                var sql = "SELECT * FROM PersonHousehold WHERE PersonId IN (" + string.Join(",", personIds) + ")";
                personHouseholds = await cmdPersonHousehold.SetCommandText(sql)
                    .ExecuteToListAsync<PersonHousehold>();
            }

            var householdIds = personHouseholds.Select(ph => ph.HouseholdId).ToList();
            if (householdIds.Count > 0)
            {
                var connHouseholdAddress = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmdHouseholdAddress = Sqlocity.GetDatabaseCommand(connHouseholdAddress);
                var sql = "SELECT * FROM HouseholdAddress WHERE HouseholdId IN (" + string.Join(",", householdIds) + ")";
                householdAddresses = await cmdHouseholdAddress.SetCommandText(sql)
                    .ExecuteToListAsync<HouseholdAddress>();
            }

            var addressIds = householdAddresses.Where(ha => ha.AddressId != null).Select(ha => ha.AddressId).ToList();
            if (addressIds.Count > 0)
            {
                var connAddress = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmdAddress = Sqlocity.GetDatabaseCommand(connAddress);
                var sql = "SELECT * FROM Address WHERE AddressId IN (" + string.Join(",", addressIds) + ")";
                addresses = await cmdAddress.SetCommandText(sql)
                    .ExecuteToListAsync<Address>();
            }

            var internalAddressIds = householdAddresses.Where(ha => ha.InternalAddressId != null).Select(ha => ha.InternalAddressId).ToList();
            if (internalAddressIds.Count > 0)
            {
                var connInternalAddress = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
                var cmdInternalAddress = Sqlocity.GetDatabaseCommand(connInternalAddress);
                var sql = "SELECT * FROM InternalAddress WHERE InternalAddressId IN (" + string.Join(",", internalAddressIds) + ")";
                internalAddresses = await cmdInternalAddress.SetCommandText(sql)
                    .ExecuteToListAsync<InternalAddress>();
            }
        }

        // now fill in the addresses and internal addresses for the householdaddresses
        foreach (var ha in householdAddresses)
        {
            if (ha.AddressId != null)
            {
                ha.Address = addresses.Where(a => a.AddressId == ha.AddressId).FirstOrDefault();
            }
            if (ha.InternalAddressId != null)
            {
                ha.InternalAddress = internalAddresses.Where(ia => ia.InternalAddressId == ha.InternalAddressId).FirstOrDefault();
            }
        }        

        return (persons, householdAddresses);
    }

    public async Task PopulateSearchTokenTables(IConfiguration config)
    {
        using var conn = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var sql = "SELECT * FROM Address";
        var addressRows = await cmd.SetCommandText(sql)
            .ExecuteToListAsync<Address>();

        var tokenLinesProcessed = 0;
        foreach (var row in addressRows)
        {
            // for each address, tokenize the address and insert the tokens into the SearchToken table
            // use SearchToken table to put only one distinct value for each token into the table
            // then insert the SearchTokenId and AddressId into the SearchTokenAddress table
            var regex = new Regex(@"\w+");
            // do for AddressLine1, AddressLine2, City, State, Zip, and Country
            string?[] toParse = { row.AddressLine1, row.AddressLine2, row.City, row.StateProvince, row.PostalCode } ;

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
            ++tokenLinesProcessed;
            if (tokenLinesProcessed % 100 == 0)
            {
                Console.WriteLine("Processed " + tokenLinesProcessed + " lines of address tokens");
            }
        }
        ++tokenLinesProcessed;
        Console.WriteLine("Processed a total of " + tokenLinesProcessed + " lines of address tokens");

        using var conn3 = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmd3 = Sqlocity.GetDatabaseCommand(conn3);
        sql = "SELECT * FROM Person";
        var personRows = await cmd3.SetCommandText(sql)
            .ExecuteToListAsync<Person>();

        tokenLinesProcessed = 0;
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
                    .AddParameter("@personId", (int)row.PersonId!)
                    .ExecuteScalarAsync<int>();
            }
            ++tokenLinesProcessed;
            if (tokenLinesProcessed % 100 == 0)
            {
                Console.WriteLine("Processed " + tokenLinesProcessed + " lines of person tokens");
            }
        }
        Console.WriteLine("Processed " + tokenLinesProcessed + " lines of person tokens");
    }
}
