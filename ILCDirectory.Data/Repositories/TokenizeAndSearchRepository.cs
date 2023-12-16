using Microsoft.VisualBasic;
using SqlocityNetCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Constants = ILCDirectory.Data.Helpers.Constants;

namespace ILCDirectory.Data.Repositories;

public class TokenizeAndSearchRepository : ITokenizeAndSearchRepository
{
    // The SearchForPerson method could be implemented in SQL, but I chose to implement it in C#
    // because I wanted to show the steps involved in the search, and the data set in the database 
    // is small enough that it can be done in memory. There are less than 10000 persons in the database.
    public async Task<IList<Person>> SearchForPerson(IConfiguration config, string searchString)
    {
        List<SearchToken> searchTokens = new List<SearchToken>();
        // use a regex to tokenize the string into words
        var regex = new Regex(@"\w+");
        var tokens = regex.Matches(searchString.ToLower()); // all tokens are lowercase

        // eliminate duplicate tokens
        var distinctTokens = tokens.Cast<Match>().Select(m => m.Value).Distinct();

        // find all the tokens in SearchTokens table in the database
        using var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var sql = "SELECT * FROM SearchToken WHERE Token IN (" + string.Join(",", distinctTokens) + ")";
        var rows = await cmd.SetCommandText(sql)
            .ExecuteToListAsync<SearchToken>();

        // find all the tokens in SearchTokenAddress table in the database
        // where every token is matched with an AddressId
        var cmd2 = Sqlocity.GetDatabaseCommand(conn);
        var sql2 = "SELECT AddressId FROM SearchTokenAddress WHERE SearchTokenId IN (" +
            string.Join(",", rows.Select(r => r.SearchTokenId).Distinct().ToArray()) + ") " +
            "GROUP BY AddressId " + // take only the AddressIds where every token is matched
            "HAVING COUNT(*) = " + distinctTokens.Count(); // the count of tokens must equal the number of tokens searched for

        var addressIds = await cmd2.SetCommandText(sql2)
            .ExecuteToListAsync<int>();

        // get all of the HouseholdIds for the AddressIds, then get all of the PersonIds for the HouseholdIds
        var cmd3 = Sqlocity.GetDatabaseCommand(conn);
        var sql3 = "SELECT PersonId FROM HouseholdPerson WHERE HouseholdId IN (" +
            "SELECT HouseholdId FROM HouseholdAddress WHERE AddressId IN (" +
            string.Join(",", addressIds) + "))";

        var personIds = await cmd3.SetCommandText(sql3)
            .ExecuteToListAsync<int>();

        // now get the personIds from the SearchTokenPerson table using the same tokens in distinctTokens
        var cmd4 = Sqlocity.GetDatabaseCommand(conn);
        var sql4 = "SELECT PersonId FROM SearchTokenPerson WHERE SearchTokenId IN (" +
            string.Join(",", rows.Select(r => r.SearchTokenId).Distinct().ToArray()) + ") " +
            "GROUP BY PersonId " + // take only the PersonIds where every token is matched
            "HAVING COUNT(*) = " + distinctTokens.Count(); // the count of tokens must equal the number of tokens searched for

        var personIds2 = await cmd4.SetCommandText(sql4)
            .ExecuteToListAsync<int>();

        personIds.AddRange(personIds2);

        // get all of the Person rows for the PersonIds
        var cmd5 = Sqlocity.GetDatabaseCommand(conn);
        var sql5 = "SELECT * FROM Person WHERE PersonId IN (" + string.Join(",", personIds) + ")";
        var persons = await cmd5.SetCommandText(sql5)
            .ExecuteToListAsync<Person>();

        return persons;
    }

    private DbConnection GetConnection(IConfiguration config)
    {
        return Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
    }
}
