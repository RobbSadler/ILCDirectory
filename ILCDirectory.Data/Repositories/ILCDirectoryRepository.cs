using SqlocityNetCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories;

public class ILCDirectoryRepository : IILCDirectoryRepository
{
    public async Task<Person> GetPersonAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var person = await cmd.SetCommandText("SELECT * FROM Person WHERE PersonId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Person>();
        return person;
    }

    public async Task<Person> InsertPersonAsync(IConfiguration config, Person person)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var personId = await cmd.GenerateInsertForSqlServer(person,"Person")
            .ExecuteToObjectAsync<int>();
        person.PersonId = personId;
        return person;
    }

    public async Task<Person> UpdatePersonAsync(IConfiguration config, Person person)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var personId = await cmd.GenerateUpdateForSqlServer(person, new List<string>{ "PersonId" })
            .ExecuteToObjectAsync<int>();
        person.PersonId = personId;
        return person;
    }

    public async Task<IList<Person>> GetAllPersonsAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var persons = await cmd.SetCommandText("SELECT * FROM Person")
            .ExecuteToListAsync<Person>();
        return persons;
    }
    public async Task<Person> FindPersonAsync(IConfiguration config, string searchTerm)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var person = await cmd.SetCommandText("SELECT * FROM Person WHERE Id = @Id")
            .ExecuteToObjectAsync<Person>();
        return person;
    }

    public async Task<Address> GetAddressAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var address = await cmd.SetCommandText("SELECT * FROM Address WHERE AddressId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Address>();
        return address;
    }
    public async Task<IList<Address>> GetAllAddressesAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var addresses = await cmd.SetCommandText("SELECT * FROM Address")
            .ExecuteToListAsync<Address>();
        return addresses;
    }
    //public async Task<IList<Address>> FindAddressesAsync(IConfiguration config, string searchTerm)
    //{

    //}

    // Find any Address or Person that contains the search term
    //public async Task<IList<SearchResult>> FindAsync(IConfiguration config, string searchTerm)
    //{
    //    var conn = GetConnection(config);
    //    var cmd = Sqlocity.GetDatabaseCommand(conn);
    //    // Use the AddressWord and PersonWord tables to find any Address or Person that contains the search term
    //    var results = await cmd.SetCommandText("SELECT * FROM AddressWord WHERE Word LIKE @SearchTerm")
    //        .AddParameter("@SearchTerm", $"%{searchTerm}%")
    //        .ExecuteToListAsync<SearchResult>();

    //}

    private DbConnection GetConnection(IConfiguration config)
    {
        return Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
    }

    //private void ParseAndAggregateIncomingAddress(IConfiguration config, Address address, DbConnection conn = null)
    //{
    //    conn = conn ?? GetConnection(config);
    //    // Tokenize and insert each word of the address into the AddressWord table
    //    // This will allow us to search for addresses by word
    //    if (address == null)
    //    {
    //        throw new ArgumentNullException(nameof(address));
    //    }

    //    // Get the values of the Address object and add rows to the AddressWord table
    //    // for each word in the object
    //    var addressType = address.GetType();
    //    var properties = addressType.GetProperties();
    //    foreach (var property in properties)
    //    {
    //        var value = property.GetValue(address);
    //        if (value != null)
    //        {
    //            var words = value.ToString().Split(' ');
    //            foreach (var word in words)
    //            {
    //                // Insert the word into the AddressWord table
    //                Sqlocity.GetDatabaseCommand(conn)
    //                    .SetCommandText("INSERT INTO AddressWord (AddressId, Word) VALUES (@AddressId, @Word)")
    //                    .AddParameter("@AddressId", address.AddressId)
    //                    .AddParameter("@Word", word)
    //                    .ExecuteNonQueryAsync();
    //            }
    //        }
    //    }
    //}

    //private void ParseAndAggregateIncomingPerson(IConfiguration config, Person person, DbConnection conn = null)
    //{
    //    conn = conn ?? GetConnection(config);
    //    // Tokenize and insert each word of the person's name into the PersonWord table
    //    // This will allow us to search for people by name
    //    if (person == null)
    //    {
    //        throw new ArgumentNullException(nameof(person));
    //    }

    //    // Get the values of the Person object and add rows to the PersonWord table
    //    // for each word in the object
    //    var personType = person.GetType();
    //    var properties = personType.GetProperties();
    //    foreach (var property in properties)
    //    {
    //        var value = property.GetValue(person);
    //        if (value != null)
    //        {
    //            var words = value.ToString().Split(' ');
    //            foreach (var word in words)
    //            {
    //                // Insert the word into the PersonWord table
    //                Sqlocity.GetDatabaseCommand(conn)
    //                    .SetCommandText("INSERT INTO PersonWord (PersonId, Word) VALUES (@PersonId, @Word)")
    //                    .AddParameter("@PersonId", person.PersonId)
    //                    .AddParameter("@Word", word)
    //                    .ExecuteNonQueryAsync();
    //            }
    //        }
    //    }
    //}
}
