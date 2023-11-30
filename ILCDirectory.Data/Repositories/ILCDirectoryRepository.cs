using ILCDirectory.Data.Models;
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
    public async Task<Building> GetBuildingAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var building = await cmd.SetCommandText("SELECT * FROM Building WHERE BuildingId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Building>();
        return building;
    }

    public async Task<IList<Building>> GetAllBuildingsAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var buildings = await cmd.SetCommandText("SELECT * FROM Building")
            .ExecuteToListAsync<Building>();
        return buildings;
    }

    public async Task<Building> InsertBuildingAsync(IConfiguration config, Building building, bool identityInsert = false)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        if (identityInsert && building.BuildingId != null) // If we are inserting an identity column, we need to manually create the insert statement
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT [Building] ON;");
            sql.AppendLine("INSERT INTO Building (BuildingId, BuildingCode, BuildingShortDesc, BuildingLongDesc, Notes, CreateDateTime, ModifiedDateTime, ModifiedByUserName)");
            sql.AppendLine("VALUES (@BuildingId, @BuildingCode, @BuildingShortDesc, @BuildingLongDesc, @Notes, @CreateDateTime, @ModifiedDateTime, @ModifiedByUserName)");
            sql.AppendLine("SELECT SCOPE_IDENTITY()");
            sql.AppendLine("SET IDENTITY_INSERT [Building] OFF;");
            cmd.SetCommandText(sql.ToString())
                .AddParameter("@BuildingId", building.BuildingId)
                .AddParameter("@BuildingCode", building.BuildingCode)
                .AddParameter("@BuildingShortDesc", building.BuildingShortDesc)
                .AddParameter("@BuildingLongDesc", building.BuildingLongDesc)
                .AddParameter("@Notes", building.Notes)
                .AddParameter("@CreateDateTime", building.CreateDateTime)
                .AddParameter("@ModifiedDateTime", building.ModifiedDateTime)
                .AddParameter("@ModifiedByUserName", building.ModifiedByUserName);
            
            await cmd.ExecuteScalarAsync<int>();
        }
        else
        {
            var buildingId = await cmd.GenerateInsertForSqlServer(building, "Building")
                .ExecuteToObjectAsync<int>();
            building.BuildingId = buildingId;
        }
        return building;
    }

    public async Task<Classification> GetClassificationAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var classification = await cmd.SetCommandText("SELECT * FROM Classification WHERE ClassificationId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Classification>();
        return classification;
    }

    public async Task<IList<Classification>> GetAllClassificationsAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var classifications = await cmd.SetCommandText("SELECT * FROM Classification")
            .ExecuteToListAsync<Classification>();
        return classifications;
    }

    public async Task<Classification> InsertClassificationAsync(IConfiguration config, Classification classification, bool identityInsert = false)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        if (identityInsert && classification.ClassificationId != null)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT [Classification] ON;");
            sql.AppendLine("INSERT INTO Classification (ClassificationId, ClassificationCode, Description, Notes, CreateDateTime, ModifiedDateTime, ModifiedByUserName)");
            sql.AppendLine("VALUES (@ClassificationId, @ClassificationCode, @Description, @Notes, @CreateDateTime, @ModifiedDateTime, @ModifiedByUserName)");
            sql.AppendLine("SELECT SCOPE_IDENTITY()");
            sql.AppendLine("SET IDENTITY_INSERT [Classification] OFF;");
            cmd.SetCommandText(sql.ToString())
                .AddParameter("@ClassificationId", classification.ClassificationId)
                .AddParameter("@ClassificationCode", classification.ClassificationCode)
                .AddParameter("@Description", classification.Description)
                .AddParameter("@Notes", classification.Notes)
                .AddParameter("@CreateDateTime", classification.CreateDateTime)
                .AddParameter("@ModifiedDateTime", classification.ModifiedDateTime)
                .AddParameter("@ModifiedByUserName", classification.ModifiedByUserName);

            await cmd.ExecuteScalarAsync<int>();
        }
        else
        { 
            var classificationId = await cmd.GenerateInsertForSqlServer(classification, "Classification")
                .ExecuteToObjectAsync<int>();
            classification.ClassificationId = classificationId;
        }
        return classification;
    }

    public async Task<DeliveryCodeLocation> GetDeliveryCodeLocationAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var deliveryCodeLocation = await cmd.SetCommandText("SELECT * FROM DeliveryCodeLocation WHERE DeliveryCodeLocationId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<DeliveryCodeLocation>();
        return deliveryCodeLocation;
    }

    public async Task<IList<DeliveryCodeLocation>> GetAllDeliveryCodeLocationsAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var deliveryCodeLocations = await cmd.SetCommandText("SELECT * FROM DeliveryCodeLocation")
            .ExecuteToListAsync<DeliveryCodeLocation>();
        return deliveryCodeLocations;
    }

    public async Task<DeliveryCodeLocation> InsertDeliveryCodeLocationAsync(IConfiguration config, DeliveryCodeLocation deliveryCodeLocation)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var deliveryCodeLocationId = await cmd.GenerateInsertForSqlServer(deliveryCodeLocation, "DeliveryCodeLocation")
            .ExecuteToObjectAsync<int>();
        return deliveryCodeLocation;
    }

    public async Task<Email> GetEmailAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var email = await cmd.SetCommandText("SELECT * FROM Email WHERE EmailId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Email>();
        return email;
    }

    public async Task<IList<Email>> GetAllEmailsAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var emails = await cmd.SetCommandText("SELECT * FROM Email")
            .ExecuteToListAsync<Email>();
        return emails;
    }

    public async Task<Email> InsertEmailAsync(IConfiguration config, Email email)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var emailId = await cmd.GenerateInsertForSqlServer(email, "Email")
            .ExecuteToObjectAsync<int>();
        return email;
    }

    public async Task<Vehicle> GetVehicleAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var vehicle = await cmd.SetCommandText("SELECT * FROM Vehicle WHERE VehicleId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Vehicle>();
        return vehicle;
    }

    public async Task<IList<Vehicle>> GetAllVehiclesAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var vehicles = await cmd.SetCommandText("SELECT * FROM Vehicle")
            .ExecuteToListAsync<Vehicle>();
        return vehicles;
    }

    public async Task<Vehicle> InsertVehicleAsync(IConfiguration config, Vehicle vehicle)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var vehicleId = await cmd.GenerateInsertForSqlServer(vehicle, "Vehicle")
            .ExecuteToObjectAsync<int>();
        vehicle.VehicleId = vehicleId;
        return vehicle;
    }

    public async Task<OfficeDetails> GetOfficeDetailsAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var officeDetails = await cmd.SetCommandText("SELECT * FROM OfficeDetails WHERE OfficeDetailsId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<OfficeDetails>();
        return officeDetails;
    }

    public async Task<IList<OfficeDetails>> GetAllOfficeDetailsAsync(IConfiguration config)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var officeDetailss = await cmd.SetCommandText("SELECT * FROM OfficeDetails")
            .ExecuteToListAsync<OfficeDetails>();
        return officeDetailss;
    }

    public async Task<OfficeDetails> InsertOfficeDetailsAsync(IConfiguration config, OfficeDetails officeDetails)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var officeDetailsId = await cmd.GenerateInsertForSqlServer(officeDetails, "OfficeDetails")
            .ExecuteToObjectAsync<int>();
        officeDetails.OfficeDetailsId = officeDetailsId;
        return officeDetails;
    }

    public async Task<Person> GetPersonAsync(IConfiguration config, int? id)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var person = await cmd.SetCommandText("SELECT * FROM Person WHERE PersonId = @Id")
            .AddParameter("@Id", id)
            .ExecuteToObjectAsync<Person>();
        return person;
    }

    public async Task<Person> InsertPersonAsync(IConfiguration config, Person person, bool identityInsert = false)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        if (identityInsert && person.PersonId != null)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT [Person] ON;");
            sql.AppendLine("INSERT INTO Person (PersonId, FirstName, LastName, MiddleName, Suffix, Notes, CreateDateTime, ModifiedDateTime, ModifiedByUserName)");
            sql.AppendLine("VALUES (@ClassificationId, @ClassificationCode, @Description, @Notes, @CreateDateTime, @ModifiedDateTime, @ModifiedByUserName)");
            sql.AppendLine("SELECT SCOPE_IDENTITY()");
            sql.AppendLine("SET IDENTITY_INSERT [Classification] OFF;");
            cmd.SetCommandText(sql.ToString())
                .AddParameter("@ClassificationId", classification.ClassificationId)
                .AddParameter("@ClassificationCode", classification.ClassificationCode)
                .AddParameter("@Description", classification.Description)
                .AddParameter("@Notes", classification.Notes)
                .AddParameter("@CreateDateTime", classification.CreateDateTime)
                .AddParameter("@ModifiedDateTime", classification.ModifiedDateTime)
                .AddParameter("@ModifiedByUserName", classification.ModifiedByUserName);

            await cmd.ExecuteScalarAsync<int>();
        }
        else
        {
            var personId = await cmd.GenerateInsertForSqlServer(person, "Person")
                .ExecuteToObjectAsync<int>();
            person.PersonId = personId;
        }
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

    //public async Task<IList<PersonHouseholdAddress>> GetAllPersonHouseholdAddressAsync(IConfiguration config, int skip, int take)
    //{
    //    var conn = GetConnection(config);
    //    var cmd = Sqlocity.GetDatabaseCommand(conn);
    //    var persons = await cmd.SetCommandText(@$"
    //        SELECT * FROM Person p
    //        INNER JOIN Household h
    //        ON h.")
    //        .ExecuteToListAsync<Person>();
    //    return persons;
    //}

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
    public async Task<Address> InsertAddressAsync(IConfiguration config, Address address)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        var addressId = await cmd.GenerateInsertForSqlServer(address, "Address")
            .ExecuteToObjectAsync<int>();
        address.AddressId = addressId;
        return address;
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
