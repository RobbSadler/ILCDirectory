using ILCDirectory.Data.Models;
using SqlocityNetCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ILCDirectory.Data.Repositories;

public class ILCDirectoryRepository : IILCDirectoryRepository
{
    private readonly IConfiguration _config;

    public ILCDirectoryRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<IList<T>> GetAllRowsAsync<T>(string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var rows = await cmd.SetCommandText($"SELECT * FROM {tableName}")
            .ExecuteToListAsync<T>();
        return rows;
    }

    public async Task<T> GetRowByIdAsync<T>(int id, string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var row = await cmd.SetCommandText($"SELECT * FROM {tableName} where {tableName}Id = @id")
            .AddParameter("@id", id)
            .ExecuteToObjectAsync<T>();
        return row;
    }

    public async Task<IList<T>> GetRowsByIdsAsync<T>(IList<int> ids, string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var rows = await cmd.SetCommandText($"SELECT * FROM {tableName} where {tableName}Id IN ({string.Join(",", ids)})")
            .ExecuteToListAsync<T>();
        return rows;
    }

    public async Task<T> InsertRowAsync<T>(T rowData, string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var id = await cmd.GenerateInsertForSqlServer(rowData, tableName)
            .ExecuteToObjectAsync<int>();

        // use reflection to access index property of rowData and set it to the id
        var rowDataType = rowData.GetType();
        var indexProperty = rowDataType.GetProperty($"{tableName}Id");
        indexProperty.SetValue(rowData, id);
        return rowData;
    }

    public async Task<T> UpdateRowAsync<T>(T rowData, List<string> keyNames, string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        await cmd.GenerateUpdateForSqlServer(rowData, keyNames, tableName)
            .ExecuteNonQueryAsync();

        return rowData;
    }

    public async Task DeleteRowAsync<T>(int id, string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        await cmd.SetCommandText($"DELETE FROM {tableName} WHERE {tableName}Id = @id")
            .AddParameter("@id", id)
            .ExecuteNonQueryAsync();
    }

    public async Task DeleteRowsAsync<T>(IList<int> ids, string tableName) where T : new()
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        await cmd.SetCommandText($"DELETE FROM {tableName} WHERE {tableName}Id IN ({string.Join(",", ids)})")
            .ExecuteNonQueryAsync();
    }

    public async Task<Building> InsertBuildingAsync(Building building, bool identityInsert = false)
    {
        var conn = GetConnection();
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

    public async Task<Classification> InsertClassificationAsync(Classification classification, bool identityInsert = false)
    {
        var conn = GetConnection();
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

    public async Task<Person> InsertPersonAsync(Person person, bool identityInsert = false)
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        if (identityInsert && person.PersonId != null)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT [Person] ON;");
            sql.AppendLine(@"INSERT INTO Person (
                PersonId, DDDId, Notes, ClassificationCode, Comment, DateOfBirth, Title, FirstName, 
                MiddleName, LastName, NickName, MaidenName, Suffix, Gender, LanguagesSpoken, MaritalStatus, SpousePersonId, Position, SupervisorName, WoCode, WorkgroupCode, 
                FieldOfService, IncludeInDirectory, IsDeleted, IsDeceased, ModifiedByUserName, CreateDateTime, ModifiedDateTime)");
            sql.AppendLine(@"VALUES (@PersonId, @DDDId, @Notes, @ClassificationCode, @Comment, @DateOfBirth, 
                @Title, @FirstName, @MiddleName, @LastName, @NickName, @MaidenName, @Suffix, @Gender, @LanguagesSpoken, @MaritalStatus, @SpousePersonId, 
                @Position, @SupervisorName, @WoCode, @WorkgroupCode, @FieldOfService, @IncludeInDirectory, 
                @IsDeleted, @IsDeceased, @ModifiedByUserName, @CreateDateTime, @ModifiedDateTime)");
            sql.AppendLine("SELECT SCOPE_IDENTITY()");
            sql.AppendLine("SET IDENTITY_INSERT [Person] OFF;");

            DbParameter dateOfBirthParm;
            if (person.DateOfBirth == null)
                dateOfBirthParm = cmd.CreateParameter("@DateOfBirth", DBNull.Value);
            else
                dateOfBirthParm = cmd.CreateParameter("@DateOfBirth", person.DateOfBirth);
            DbParameter titleParm;
            if (person.Title == null)
                titleParm = cmd.CreateParameter("@Title", DBNull.Value);
            else
                titleParm = cmd.CreateParameter("@Title", person.Title);
            DbParameter middleNameParm;
            if (person.MiddleName == null)
                middleNameParm = cmd.CreateParameter("@MiddleName", DBNull.Value);
            else
                middleNameParm = cmd.CreateParameter("@MiddleName", person.MiddleName);
            DbParameter lastNameParm;
            if (person.LastName == null)
                lastNameParm = cmd.CreateParameter("@LastName", DBNull.Value);
            else
                lastNameParm = cmd.CreateParameter("@LastName", person.LastName);
            DbParameter nickNameParm;
            if (person.NickName == null)
                nickNameParm = cmd.CreateParameter("@NickName", DBNull.Value);
            else
                nickNameParm = cmd.CreateParameter("@NickName", person.NickName);
            DbParameter maidenNameParm;
            if (person.MaidenName == null)
                maidenNameParm = cmd.CreateParameter("@MaidenName", DBNull.Value);
            else
                maidenNameParm = cmd.CreateParameter("@MaidenName", person.MaidenName);
            DbParameter suffixParm;
            if (person.Suffix == null)
                suffixParm = cmd.CreateParameter("@Suffix", DBNull.Value);
            else
                suffixParm = cmd.CreateParameter("@Suffix", person.Suffix);
            DbParameter languagesSpokenParm;
            if (person.LanguagesSpoken == null)
                languagesSpokenParm = cmd.CreateParameter("@LanguagesSpoken", DBNull.Value);
            else
                languagesSpokenParm = cmd.CreateParameter("@LanguagesSpoken", person.LanguagesSpoken);
            DbParameter positionParm;
            if (person.Position == null)
                positionParm = cmd.CreateParameter("@Position", DBNull.Value);
            else
                positionParm = cmd.CreateParameter("@Position", person.Position);
            DbParameter supervisorNameParm;
            if (person.SupervisorName == null)
                supervisorNameParm = cmd.CreateParameter("@SupervisorName", DBNull.Value);
            else
                supervisorNameParm = cmd.CreateParameter("@SupervisorName", person.Position);
            DbParameter woCodeParm;
            if (person.WoCode == null)
                woCodeParm = cmd.CreateParameter("@WoCode", DBNull.Value);
            else
                woCodeParm = cmd.CreateParameter("@WoCode", person.WoCode);
            DbParameter maritalStatusParm;
            if (person.MaritalStatus == null)
                maritalStatusParm = cmd.CreateParameter("@MaritalStatus", DBNull.Value);
            else
                maritalStatusParm = cmd.CreateParameter("@MaritalStatus", person.MaritalStatus);
            DbParameter spousePersonIdParm;
            if (person.SpousePersonId == null)
                spousePersonIdParm = cmd.CreateParameter("@SpousePersonId", DBNull.Value);
            else
                spousePersonIdParm = cmd.CreateParameter("@SpousePersonId", person.SpousePersonId);
            DbParameter workgroupCodeParm;
            if (person.WorkgroupCode == null)
                workgroupCodeParm = cmd.CreateParameter("@WorkgroupCode", DBNull.Value);
            else
                workgroupCodeParm = cmd.CreateParameter("@WorkgroupCode", person.WorkgroupCode);
            DbParameter fieldOfServiceParm;
            if (person.FieldOfService == null)
                fieldOfServiceParm = cmd.CreateParameter("@FieldOfService", DBNull.Value);
            else
                fieldOfServiceParm = cmd.CreateParameter("@FieldOfService", person.FieldOfService);

            cmd.SetCommandText(sql.ToString())
                .AddParameter("@PersonId", person.PersonId)
                .AddParameter("@DDDId", person.DDDId)
                .AddParameter("@Notes", person.Notes)
                .AddParameter("@ClassificationCode", person.ClassificationCode)
                .AddParameter("@Comment", person.Comment)
                .AddParameter("@FirstName", person.FirstName)
                .AddParameter("@Gender", person.Gender)
                .AddParameter("@IncludeInDirectory", person.IncludeInDirectory)
                .AddParameter("@IsDeleted", person.IsDeleted)
                .AddParameter("@IsDeceased", person.IsDeceased)
                .AddParameter("@ModifiedByUserName", person.ModifiedByUserName)
                .AddParameter("@CreateDateTime", person.CreateDateTime)
                .AddParameter("@ModifiedDateTime", person.ModifiedDateTime)

                // nullable parameters
                .AddParameter(dateOfBirthParm)
                .AddParameter(titleParm)
                .AddParameter(middleNameParm)
                .AddParameter(lastNameParm)
                .AddParameter(nickNameParm)
                .AddParameter(maidenNameParm)
                .AddParameter(suffixParm)
                .AddParameter(languagesSpokenParm)
                .AddParameter(positionParm)
                .AddParameter(supervisorNameParm)
                .AddParameter(woCodeParm)
                .AddParameter(maritalStatusParm)
                .AddParameter(spousePersonIdParm)
                .AddParameter(workgroupCodeParm)
                .AddParameter(fieldOfServiceParm);

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

    public async Task<DeliveryCodeLocation> InsertDeliveryCodeLocationAsync(DeliveryCodeLocation deliveryCodeLocation, bool identityInsert = false)
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        if (identityInsert)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT [DeliveryCodeLocation] ON;");
            sql.AppendLine(@"INSERT INTO DeliveryCodeLocation (
                [DeliveryCodeLocationId], [DeliveryCode], [DeliveryLocation], [ModifiedByUserName])
                VALUES (@DeliveryCodeLocationId, @DeliveryCode, @DeliveryLocation, @ModifiedByUserName)");
            sql.AppendLine("SELECT SCOPE_IDENTITY()");
            sql.AppendLine("SET IDENTITY_INSERT [DeliveryCodeLocation] OFF;");

            cmd.SetCommandText(sql.ToString())
                .AddParameter("@DeliveryCodeLocationId", deliveryCodeLocation.DeliveryCodeLocationId)
                .AddParameter("@DeliveryCode", deliveryCodeLocation.DeliveryCode)
                .AddParameter("@DeliveryLocation", deliveryCodeLocation.DeliveryLocation)
                .AddParameter("@ModifiedByUserName", deliveryCodeLocation.ModifiedByUserName);

            await cmd.ExecuteScalarAsync<int>();
        }
        else
        {
            deliveryCodeLocation.DeliveryCodeLocationId = await cmd.GenerateInsertForSqlServer(deliveryCodeLocation, "DeliveryCodeLocation")
                .ExecuteToObjectAsync<int>();
        }
        return deliveryCodeLocation;
    }

    public async Task<List<ParentChild>> GetParentChildFromParentIdAsync(int parentId)
    {
        // get all parent child relationships in ParentChild table where ParentId = parentId
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parentChilds = await cmd.SetCommandText("SELECT * FROM ParentChild WHERE ParentId = @ParentId")
            .AddParameter("@ParentId", parentId)
            .ExecuteToListAsync<ParentChild>();
        return parentChilds;
    }

    public async Task<List<ParentChild>> GetParentChildFromChildIdAsync(int childId)
    {
        // get all parent child relationships in ParentChild table where ChildId = childId
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parentChilds = await cmd.SetCommandText("SELECT * FROM ParentChild WHERE ChildId = @ChildId")
            .AddParameter("@ChildId", childId)
            .ExecuteToListAsync<ParentChild>();
        return parentChilds;
    }

    public async Task<PersonFamilyAddressDetails> GetPersonFamilyAddressDetailsAsync(int personId, int? spousePersonId)
    {
        var personFamilyAddressDetails = new PersonFamilyAddressDetails();

        var person = await GetRowByIdAsync<Person>(personId, "Person");
        personFamilyAddressDetails.Person = person;

        var personFamilyDetails = await GetPersonFamilyDetailsAsync(personId, spousePersonId);
        personFamilyAddressDetails.PersonFamilyDetails = personFamilyDetails;

        var personAddressDetails = await GetPersonAddressDetailsAsync(personId);
        personFamilyAddressDetails.PersonAddressDetails = personAddressDetails;

        return personFamilyAddressDetails;
    }

    public async Task<PersonFamilyDetails> GetPersonFamilyDetailsAsync(int personId, int? spousePersonId)
    {
        var personFamilyDetails = new PersonFamilyDetails();

        var personEmails = await GetPersonEmailsAsync(personId);
        personFamilyDetails.PersonEmails.Add(personId, personEmails);

        // Get ParentPersons
        var parentPersons = await GetParentsForPersonAsync(personId);
        personFamilyDetails.ParentPersons = parentPersons;

        // Get ChildPersons
        var childPersons = await GetChildrenForPersonAsync(personId);
        personFamilyDetails.ChildPersons = childPersons;

        if (spousePersonId != null)
        {
            var spouse = await GetRowByIdAsync<Person>((int)spousePersonId, "Person");
            personFamilyDetails.Spouse = spouse;
            var spousePersonPhones = await GetPersonPhonesAsync((int)spousePersonId!);
            personFamilyDetails.PersonPhones.Add((int)spousePersonId, spousePersonPhones);
            var spousePersonEmails = await GetPersonEmailsAsync((int)spousePersonId!);
            personFamilyDetails.PersonEmails.Add((int)spousePersonId, spousePersonEmails);
        }

        if (parentPersons != null && parentPersons.Count > 0)
        {
            foreach (var parentPerson in parentPersons)
            {
                var parentPersonPhones = await GetPersonPhonesAsync((int)parentPerson.PersonId!);
                personFamilyDetails.PersonPhones.Add((int)parentPerson.PersonId, parentPersonPhones);
                var parentPersonEmails = await GetPersonEmailsAsync((int)parentPerson.PersonId!);
                personFamilyDetails.PersonEmails.Add((int)parentPerson.PersonId, parentPersonEmails);
            }
        }
        if (childPersons != null && childPersons.Count > 0)
        {
            foreach (var childPerson in childPersons)
            {
                var childPersonPhones = await GetPersonPhonesAsync((int)childPerson.PersonId!);
                personFamilyDetails.PersonPhones.Add((int)childPerson.PersonId, childPersonPhones);
                var childPersonEmails = await GetPersonEmailsAsync((int)childPerson.PersonId!);
                personFamilyDetails.PersonEmails.Add((int)childPerson.PersonId, childPersonEmails);
            }
        }
        return personFamilyDetails;
    }

    public async Task<PersonAddressDetails> GetPersonAddressDetailsAsync(int personId)
    {
        var personAddressDetails = new PersonAddressDetails();
        // Get PersonHouseholds
        var personHouseholds = await GetPersonHouseholdsAsync(personId);
        personAddressDetails.Households = personHouseholds;

        // Get PersonHouseholdAddresses
        var personHouseholdAddresses = await GetHouseholdAddressesForPersonAsync(personId);
        personAddressDetails.HouseholdAddresses = personHouseholdAddresses;

        foreach (var pha in personHouseholdAddresses)
        {
            if (pha.AddressId != null)
                pha.Address = await GetRowByIdAsync<Address>(pha.AddressId!.Value, "Address");
            if (pha.InternalAddressId != null)
                pha.InternalAddress = await GetRowByIdAsync<InternalAddress>(pha.InternalAddressId!.Value, "InternalAddress");
        }

        return personAddressDetails;
    }

    public async Task<IList<Email>> GetPersonEmailsAsync(int personId)
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var personEmails = await cmd.SetCommandText("SELECT * FROM Email WHERE PersonId = @PersonId")
            .AddParameter("@PersonId", personId)
            .ExecuteToListAsync<Email>();
        return personEmails;
    }

    public async Task<List<Person>> GetParentPersonsFromChildAsync(int childId)
    {
        // get all parent child relationships in ParentChild table where ChildId = childId then retrieve person from Person table where PersonId = ParentId
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parents = await cmd.SetCommandText(@"SELECT * FROM Person 
            WHERE PersonId IN (SELECT ParentId FROM ParentChild WHERE ChildId = @ChildId)")
            .AddParameter("@childId", childId)
            .ExecuteToListAsync<Person>();
        return parents;
    }

    public async Task<List<PhoneNumber>> GetPersonPhonesAsync(int personId)
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var phones = await cmd.SetCommandText(@"SELECT * FROM PhoneNumber 
            WHERE PersonId = @personId")
            .AddParameter("@personId", personId)
            .ExecuteToListAsync<PhoneNumber>();
        return phones;
    }

    public async Task<List<PersonHousehold>> GetPersonHouseholdsAsync(int personId)
    {
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var personHouseholds = await cmd.SetCommandText(@"SELECT * FROM PersonHousehold 
            WHERE PersonId = @personId")
            .AddParameter("@personId", personId)
            .ExecuteToListAsync<PersonHousehold>();
        return personHouseholds;
    }

    public async Task<List<Person>> GetChildPersonsForParentAsync(int parentId)
    {
        // get all parent child relationships in ParentChild table where ChildId = childId then retrieve person from Person table where PersonId = ParentId
        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var children = await cmd.SetCommandText(@"SELECT * FROM Person 
            WHERE PersonId IN (SELECT ChildId FROM ParentChild WHERE ParentId = @parentId)")
            .AddParameter("@parentId", parentId)
            .ExecuteToListAsync<Person>();
        return children;
    }

    public async Task<InternalAddress> GetInternalAddressForPersonAsync(int personId)
    {
        var sql = @$"
            SELECT * FROM InternalAddress ia
            WHERE ia.PersonId = @personId";

        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var internalAddress = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToObjectAsync<InternalAddress>();
        return internalAddress;
    }

    public async Task<Household> GetHouseholdForPersonAsync(int personId)
    {
        var sql = @$"
            SELECT * FROM Household h
            INNER JOIN PersonHousehold ph
            ON ph.HouseholdId = h.HouseholdId
            WHERE ph.PersonId = @personId";

        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var household = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToObjectAsync<Household>();
        return household;
    }

    // We don't need to get persons from this query since they will be part of the parent / child relationship
    // but we'll get the personIds
    public async Task<List<int>> GetPersonIdsForHouseholdAsync(int householdId)
    {
        var sql = @$"
            SELECT PersonId FROM PersonHousehold ph
            WHERE ph.HouseholdId = @householdId";

        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var personIds = await cmd.SetCommandText(sql)
            .AddParameter("@householdId", householdId)
            .ExecuteToListAsync<int>();
        return personIds;
    }

    public async Task<List<HouseholdAddress>> GetHouseholdAddressesForPersonAsync(int personId)
    {
        var sql = @$"
            SELECT * FROM HouseholdAddress ha
            INNER JOIN PersonHousehold ph ON ph.HouseholdId = ha.HouseholdId
            WHERE ph.PersonId = @personId";

        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var householdAddress = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToListAsync<HouseholdAddress>();
        return householdAddress;

        foreach (var ha in householdAddress)
        {
            var address = await GetRowByIdAsync<Address>(ha.AddressId.Value, "Address");
            ha.Address = address;
        }
    }

    public async Task<List<Person>> GetChildrenForPersonAsync(int personId)
    {
        var sql = @$"
            SELECT p.* FROM Person p
            INNER JOIN ParentChild pc ON pc.ChildId = p.PersonId
            WHERE pc.ParentId = @personId";

        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var children = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToListAsync<Person>();
        return children;
    }

    public async Task<List<Person>> GetParentsForPersonAsync(int personId)
    {
        var sql = @$"
            SELECT * FROM Person p
            INNER JOIN ParentChild pc ON pc.ParentId = p.PersonId
            WHERE pc.ChildId = @personId";

        var conn = GetConnection();
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parents = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToListAsync<Person>();
        return parents;
    }

    private DbConnection GetConnection()
    {
        return Sqlocity.CreateDbConnection(_config[Constants.CONFIG_CONNECTION_STRING]);
    }
}
