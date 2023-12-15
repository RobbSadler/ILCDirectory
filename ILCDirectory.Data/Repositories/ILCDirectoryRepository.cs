using ILCDirectory.Data.Models;
using SqlocityNetCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Azure.Core.HttpHeader;

namespace ILCDirectory.Data.Repositories;

public class ILCDirectoryRepository : IILCDirectoryRepository
{
    public async Task<IList<T>> GetAllRowsAsync<T>(IConfiguration config, int id, string tableName) where T : new()
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var rows = await cmd.SetCommandText($"SELECT * FROM {tableName}")
            .ExecuteToListAsync<T>();
        return rows;
    }

    public async Task<T> GetRowByIdAsync<T>(IConfiguration config, int id, string tableName) where T : new()
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var row = await cmd.SetCommandText($"SELECT * FROM {tableName} where {tableName}Id = @id")
            .AddParameter("@id", id)
            .ExecuteToObjectAsync<T>();
        return row;
    }

    public async Task<T> InsertRowAsync<T>(IConfiguration config, T rowData, string tableName) where T : new()
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var id = await cmd.GenerateInsertForSqlServer(rowData, tableName)
            .ExecuteToObjectAsync<int>();

        // use reflection to access index property of rowData and set it to the id
        var rowDataType = rowData.GetType();
        var indexProperty = rowDataType.GetProperty($"{tableName}Id");
        indexProperty.SetValue(rowData, id);
        return rowData;
    }

    public async Task<IList<T>> GetAllRowsAsync<T>(IConfiguration config, string tableName) where T : new()
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var rows = await cmd.SetCommandText($"SELECT * FROM {tableName}")
            .ExecuteToListAsync<T>();
        return rows;
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

    public async Task<Person> InsertPersonAsync(IConfiguration config, Person person, bool identityInsert = false)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        if (identityInsert && person.PersonId != null)
        {
            var sql = new StringBuilder();
            sql.AppendLine("SET IDENTITY_INSERT [Person] ON;");
            sql.AppendLine(@"INSERT INTO Person (
                PersonId, DDDId, Notes, ClassificationCode, Comment, DateOfBirth, DeleteFlag, DirCorrFormNote, DirectoryCorrectionForm, Title, FirstName, 
                MiddleName, LastName, NickName, MaidenName, Suffix, Gender, LanguagesSpoken, MaritalStatus, Position, WoCode, WorkgroupCode, 
                IncludeInDirectory, ModifiedByUserName, CreateDateTime, ModifiedDateTime)");
            sql.AppendLine(@"VALUES (@PersonId, @DDDId, @Notes, @ClassificationCode, @Comment, @DateOfBirth, @DeleteFlag, @DirCorrFormNote, @DirectoryCorrectionForm, 
                @Title, @FirstName, @MiddleName, @LastName, @NickName, @MaidenName, @Suffix, @Gender, @LanguagesSpoken, @MaritalStatus, @Position, @WoCode, 
                @WorkgroupCode, @IncludeInDirectory, @ModifiedByUserName, @CreateDateTime, @ModifiedDateTime)");
            sql.AppendLine("SELECT SCOPE_IDENTITY()");
            sql.AppendLine("SET IDENTITY_INSERT [Classification] OFF;");

            DbParameter dateOfBirthParm;
            if (person.DateOfBirth == null)
                dateOfBirthParm = cmd.CreateParameter("@DateOfBirth", DBNull.Value);
            else
                dateOfBirthParm = cmd.CreateParameter("@DateOfBirth", person.DateOfBirth);
            DbParameter dirCorrFormNoteParm;
            if (person.DirCorrFormNote == null)
                dirCorrFormNoteParm = cmd.CreateParameter("@DirCorrFormNote", DBNull.Value);
            else
                dirCorrFormNoteParm = cmd.CreateParameter("@DirCorrFormNote", person.DirCorrFormNote);
            DbParameter directoryCorrectionFormParm;
            if (person.DirectoryCorrectionForm == null)
                directoryCorrectionFormParm = cmd.CreateParameter("@DirectoryCorrectionForm", DBNull.Value);
            else
                directoryCorrectionFormParm = cmd.CreateParameter("@DirectoryCorrectionForm", person.DirectoryCorrectionForm);
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
            DbParameter workgroupCodeParm;
            if (person.WorkgroupCode == null)
                workgroupCodeParm = cmd.CreateParameter("@WorkgroupCode", DBNull.Value);
            else
                workgroupCodeParm = cmd.CreateParameter("@WorkgroupCode", person.WorkgroupCode);

            cmd.SetCommandText(sql.ToString())
                .AddParameter("@PersonId", person.PersonId)
                .AddParameter("@DDDId", person.DDDId)
                .AddParameter("@Notes", person.Notes)
                .AddParameter("@ClassificationCode", person.ClassificationCode)
                .AddParameter("@Comment", person.Comment)
                .AddParameter("@DeleteFlag", person.DeleteFlag)
                .AddParameter("@FirstName", person.FirstName)
                .AddParameter("@Gender", person.Gender)
                .AddParameter("@IncludeInDirectory", person.IncludeInDirectory)
                .AddParameter("@ModifiedByUserName", person.ModifiedByUserName)
                .AddParameter("@CreateDateTime", person.CreateDateTime)
                .AddParameter("@ModifiedDateTime", person.ModifiedDateTime)

                // nullable parameters
                .AddParameter(dateOfBirthParm)
                .AddParameter(dirCorrFormNoteParm)
                .AddParameter(directoryCorrectionFormParm)
                .AddParameter(titleParm)
                .AddParameter(middleNameParm)
                .AddParameter(lastNameParm)
                .AddParameter(nickNameParm)
                .AddParameter(maidenNameParm)
                .AddParameter(suffixParm)
                .AddParameter(languagesSpokenParm)
                .AddParameter(positionParm)
                .AddParameter(woCodeParm)
                .AddParameter(maritalStatusParm)
                .AddParameter(workgroupCodeParm);

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

    public async Task<DeliveryCodeLocation> InsertDeliveryCodeLocationAsync<DeliveryCodeLocation>(IConfiguration config, DeliveryCodeLocation deliveryCodeLocation)
    {
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);

        await cmd.GenerateInsertForSqlServer(deliveryCodeLocation, "DeliveryCodeLocation")
            .ExecuteToObjectAsync<int>();
        return deliveryCodeLocation;
    }

    public async Task<List<ParentChild>> GetParentChildFromParentIdAsync(IConfigurationRoot config, int parentId)
    {
        // get all parent child relationships in ParentChild table where ParentId = parentId
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parentChilds = await cmd.SetCommandText("SELECT * FROM ParentChild WHERE ParentId = @ParentId")
            .AddParameter("@ParentId", parentId)
            .ExecuteToListAsync<ParentChild>();
        return parentChilds;
    }

    public async Task<List<ParentChild>> GetParentChildFromChildIdAsync(IConfigurationRoot config, int childId)
    {
        // get all parent child relationships in ParentChild table where ChildId = childId
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parentChilds = await cmd.SetCommandText("SELECT * FROM ParentChild WHERE ChildId = @ChildId")
            .AddParameter("@ChildId", childId)
            .ExecuteToListAsync<ParentChild>();
        return parentChilds;
    }

    public async Task<List<Person>> GetParentPersonsFromChildAsync(IConfigurationRoot config, int childId)
    {
        // get all parent child relationships in ParentChild table where ChildId = childId then retrieve person from Person table where PersonId = ParentId
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var parents = await cmd.SetCommandText(@"SELECT * FROM Person 
            WHERE PersonId IN (SELECT ParentId FROM ParentChild WHERE ChildId = @ChildId)")
            .AddParameter("@childId", childId)
            .ExecuteToListAsync<Person>();
        return parents;
    }

    public async Task<List<Person>> GetChildPersonsForParentAsync(IConfigurationRoot config, int parentId)
    {
        // get all parent child relationships in ParentChild table where ChildId = childId then retrieve person from Person table where PersonId = ParentId
        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var children = await cmd.SetCommandText(@"SELECT * FROM Person 
            WHERE PersonId IN (SELECT ChildId FROM ParentChild WHERE ParentId = @parentId)")
            .AddParameter("@parentId", parentId)
            .ExecuteToListAsync<Person>();
        return children;
    }

    public async Task<InternalAddress> GetInternalAddressForPerson(IConfiguration config, int personId)
    {
        var sql = @$"
            SELECT * FROM InternalAddress ia
            WHERE ia.PersonId = @personId";

        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var internalAddress = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToObjectAsync<InternalAddress>();
        return internalAddress;
    }

    public async Task<Household> GetHouseholdForPerson(IConfiguration config, int personId)
    {
        var sql = @$"
            SELECT * FROM Household h
            INNER JOIN PersonHousehold ph
            ON ph.HouseholdId = h.HouseholdId
            WHERE ph.PersonId = @personId";

        var conn = GetConnection(config);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var household = await cmd.SetCommandText(sql)
            .AddParameter("@personId", personId)
            .ExecuteToObjectAsync<Household>();
        return household;
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

    private DbConnection GetConnection(IConfiguration config)
    {
        return Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
    }
}
