using ILCDirectory.Data.Helpers;
using ILCDirectory.Data.Models;
using ILCDirectory.Data.Repositories;
using Microsoft.Data.SqlClient;
using SqlocityNetCore;
using System.Data.Common;

using (OleDbConnection connect = new OleDbConnection(
        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\git\\SIL\\DDD-Tables.accdb;Persist Security Info=False;"))
{
    DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);
    var configurationBuilder = new ConfigurationBuilder();
    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    configurationBuilder.AddUserSecrets(typeof(Program).GetTypeInfo().Assembly, optional: false);
    IConfigurationRoot config = configurationBuilder.Build();
    var modifiedByUserName = "ILCDirectoryMigrator";

    // lookup dictionaries
    var titleDictionary = new Dictionary<int, string>();
    var classificationDictionary = new Dictionary<int, Classification>();
    var repo = new ILCDirectoryRepository();

    connect.Open();

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Building

    OleDbCommand cmdBuilding = new OleDbCommand("select * from tblBuildings", connect);
    OleDbDataAdapter daBuilding = new OleDbDataAdapter(cmdBuilding);
    DataSet dsetBuilding = new DataSet();
    daBuilding.Fill(dsetBuilding);
    var allCurrentBuildingRows = await repo.GetAllBuildingsAsync(config);
    foreach (DataRow buildingRow in dsetBuilding.Tables[0].Rows)
    {
        var building = new Building()
        {
            BuildingId = (int)buildingRow["BuildingID"],
            BuildingCode = buildingRow["BuildingCode"].ToString(),
            BuildingLongDesc = buildingRow["BuildingLongDesc"].ToString(),
            BuildingShortDesc = buildingRow["BuildingShortDesc"].ToString(),
            ModifiedByUserName = "Data Migrator",
        };
        if (!allCurrentBuildingRows.Any(b => b.BuildingId == building.BuildingId))
            await repo.InsertBuildingAsync(config, building, true);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // USCityInfo - create a temporary dictionary to look up city codes and map to city names as we populate addresses
    var cityCodeCity = new Dictionary<string, string>();
    OleDbCommand cmdCityCode = new OleDbCommand("select * from tblCityCodes", connect);
    OleDbDataAdapter daCityCode = new OleDbDataAdapter(cmdCityCode);
    DataSet dsetCityCode = new DataSet();
    daCityCode.Fill(dsetCityCode);
    foreach (DataRow cityCodeRow in dsetCityCode.Tables[0].Rows)
    {
        if (cityCodeRow["CityCode"] == DBNull.Value || cityCodeRow["CityCode"].ToString() == "---")
            continue;

        if (!cityCodeCity.ContainsKey(cityCodeRow["CityCode"].ToString()))
            cityCodeCity.Add(cityCodeRow["CityCode"].ToString(), cityCodeRow["CityLongDesc"].ToString());
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Classification
    OleDbCommand cmdClassification = new OleDbCommand("select * from tblClassification", connect);
    OleDbDataAdapter daClassification = new OleDbDataAdapter(cmdClassification);
    DataSet dsetClassification = new DataSet();
    daClassification.Fill(dsetClassification);
    var allCurrentClassificationRows = await repo.GetAllClassificationsAsync(config);
    foreach (DataRow classificationRow in dsetClassification.Tables[0].Rows)
    {
        if (classificationRow["StatusCode"] == DBNull.Value)
            continue;

        var classification = new Classification()
        {
            ClassificationId = (int)classificationRow["ClassificationID"],
            ClassificationCode = (string)classificationRow["StatusCode"],
            Description = (string)classificationRow["StatusDescription"],
            ModifiedDateTime = DateTime.Now,
            ModifiedByUserName = modifiedByUserName
        };

        if (!classificationDictionary.ContainsKey((int)classificationRow["ClassificationId"]))
            classificationDictionary.Add((int)classificationRow["ClassificationId"], classification);

        if (!allCurrentClassificationRows.Any(c => c.ClassificationId == classification.ClassificationId))
            await repo.InsertClassificationAsync(config, classification, true);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // DeliveryCodeDescription
    OleDbCommand cmdDeliveryCodeLocation = new OleDbCommand("select * from tblMailDelivery", connect);
    OleDbDataAdapter daDeliveryCodeLocation = new OleDbDataAdapter(cmdDeliveryCodeLocation);
    DataSet dsetDeliveryCodeLocation = new DataSet();
    daDeliveryCodeLocation.Fill(dsetDeliveryCodeLocation);
    var allCurrentDeliveryCodeLocationRows = await repo.GetAllDeliveryCodeLocationsAsync(config);
    // this table contains duplicate delivery codes, so we need to combine the delivery location values into a single string
    // and then insert a single row for each delivery code
    Dictionary<string, string> deliveryCodeLocation = new ();
    foreach (DataRow row in dsetDeliveryCodeLocation.Tables[0].Rows)
    {
        if (row["DeliveryCode"] == DBNull.Value)
            continue;

        if (!deliveryCodeLocation.ContainsKey((string)row["DeliveryCode"]))
            deliveryCodeLocation.Add((string)row["DeliveryCode"], (string)row["DeliveryLocation"]);
        else
            deliveryCodeLocation[(string)row["DeliveryCode"]] += ", " + (string)row["DeliveryLocation"];
    }
    foreach (KeyValuePair<string, string> item in deliveryCodeLocation)
    {
        if (allCurrentDeliveryCodeLocationRows.Any(d => d.DeliveryCode == item.Key))
            continue;

        var rowdata = new DeliveryCodeLocation()
        {
            DeliveryCode = item.Key,
            DeliveryLocation = item.Value,
            ModifiedByUserName = modifiedByUserName
        };
        
        await repo.InsertDeliveryCodeLocationAsync(config, rowdata);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Email - inserted below from address where they existed previously 

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Title
    OleDbCommand cmdTitle = new OleDbCommand("select * from tblTitle", connect);
    OleDbDataAdapter daTitle = new OleDbDataAdapter(cmdTitle);
    DataSet dsetTitle = new DataSet();
    daTitle.Fill(dsetTitle);
    foreach (DataRow titleRow in dsetTitle.Tables[0].Rows)
    {
        if (!titleDictionary.ContainsKey((int)titleRow["TitleId"]))
            titleDictionary.Add((int)titleRow["TitleId"], (string)titleRow["TitleName"]);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Person

    OleDbCommand personCmd = new OleDbCommand("select * from Person", connect);
    OleDbDataAdapter daPerson = new OleDbDataAdapter(personCmd);
    DataSet dsetPerson = new DataSet();
    daPerson.Fill(dsetPerson);

    var allPersonRows = await repo.GetAllPersonsAsync(config);
    var allAddressRows = await repo.GetAllAddressesAsync(config);
    var allOfficeDetailsRows = await repo.GetAllOfficeDetailsAsync(config);

    foreach(DataRow srcRow in dsetPerson.Tables[0].Rows)
    {
        if ((bool)srcRow["DeleteFlag"] == true)
            continue;

        var person = new Person();
        person.DDDId = (int)srcRow["ID"];
        person.Notes = srcRow["AuditTrail"].ToString();
        person.ClassificationCode = classificationDictionary[Convert.ToInt32((string)srcRow["StatusCode"])].ClassificationCode;
        person.Comment = srcRow["Comment"].ToString();
        person.DateOfBirth = (DateTime?)srcRow["BirthDate"];
        person.DeleteFlag = false;
        person.DirCorrFormNote = srcRow["DirCorrFormNote"].ToString();
        person.DirectoryCorrectionForm = srcRow["DirectoryCorrectionForm"] == DBNull.Value ? null : (DateTime)srcRow["DirectoryCorrectionForm"];
        person.Title = srcRow["Title"] == DBNull.Value ? "" : titleDictionary[Convert.ToInt32((string)srcRow["Title"])];
        person.FirstName = srcRow["FirstName"].ToString();
        person.MiddleName = srcRow["MiddleName"].ToString();
        person.LastName = srcRow["LastName"].ToString();
        person.NickName = srcRow["NickName"].ToString();
        person.MaidenName = srcRow["MaidenName"].ToString();
        person.Suffix = srcRow["Suffix"].ToString();
        person.Gender = srcRow["Gender"].ToString();
        person.LanguagesSpoken = srcRow["LanguagesSpoken"].ToString();
        person.MaritalStatus = srcRow["MaritalStatus"].ToString();
        person.Position = srcRow["Position"].ToString();
        person.WoCode = srcRow["WO_Code"].ToString();
        person.WorkgroupCode = (int?)srcRow["WorkgroupCode"];
        person.IncludeInDirectory = (bool)srcRow["DirectoryInclude"];

        person.ModifiedByUserName = modifiedByUserName;
        person.CreateDateTime = DateTimeOffset.Now;
        person.ModifiedDateTime = DateTimeOffset.Now;

        if (!allPersonRows.Any(p => p.DDDId == person.DDDId))
        { 
            await repo.InsertPersonAsync(config, person, );

            // Check if person has spouse or children, and if they currently exist (thus, have a household row)
            // add household row
            var household = new Household();

        }

        // get personId so we can associate below
        var conn = Sqlocity.CreateDbConnection(config[Constants.CONFIG_CONNECTION_STRING]);
        var cmd = Sqlocity.GetDatabaseCommand(conn);
        var personId = await cmd.SetCommandText("select PersonId from Person where DDDId = @DDDId")
            .AddParameter("@DDDId", person.DDDId)
            .ExecuteScalarAsync<int>();

        // pull out directory address info
        var address = new Address();
        address.AddressLine1 = srcRow["DirectoryAddress"].ToString();
        address.City = srcRow["DirectoryCity"].ToString();
        address.ZipCode = srcRow["DirectoryZIP"].ToString();

        address.RoomNumber = srcRow["DirectoryRoom"].ToString();
        address.MailListFlag = (bool)srcRow["MailListFlag"];
        address.MailOnly = (bool)srcRow["MailOnly"];
        address.MailSortName = srcRow["MailSortName"].ToString();
        address.SpecialContactInfo = srcRow["SpecialContactInfo"].ToString();
        address.IsActive = true;                                    // TODO: which way to default?
        address.IsVerified = true;

        if (!allAddressRows.Any(a => a.AddressLine1 == address.AddressLine1 && a.City == address.City && a.ZipCode == address.ZipCode))
            await repo.InsertAddressAsync(config, address);

        // pull out Office Info
        var officeDetails = new OfficeDetails();
        
        officeDetails.DDDId = (int)srcRow["ID"];
        officeDetails.PersonId = personId;
        officeDetails.BuildingId = (int?)srcRow["BuildingCode"];
        officeDetails.CubicleNumber = srcRow["CubicleNumber"].ToString();
        officeDetails.RoomNumber = srcRow["RoomNumber"].ToString();
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Address
    // addr repo config'd above
    OleDbCommand addressCmd = new OleDbCommand("select * from Addresses", connect);
    OleDbDataAdapter daAddress = new OleDbDataAdapter(addressCmd);
    DataSet dsetAddress = new DataSet();
    daAddress.Fill(dsetAddress);

    foreach (DataRow srcRow in dsetAddress.Tables[0].Rows)
    {
        var address = new Address();
        address.DDDId = (int)srcRow["ID"];
        address.Notes = srcRow["AuditTrail"].ToString();
        address.AddressLine1 = srcRow["AddressLine1"].ToString();
        address.AddressLine2 = srcRow["AddressLine2"].ToString();
        address.AddressLine3 = srcRow["AddressLine3"].ToString();
        address.AddressLine4 = srcRow["AddressLine4"].ToString();
        address.City = srcRow["City"].ToString();
        address.ContactPersonId = (int)srcRow["ContactPersonId"];

        address.IncludeInDirectory = (bool)srcRow["IncludeInDirectory"];
        address.CreateDateTime = DateTimeOffset.Now;
        address.ModifiedDateTime = DateTimeOffset.Now;
        address.ModifiedByUserName = modifiedByUserName;
        await repo.InsertAddressAsync(config, address);

        if (srcRow["Email"] == DBNull.Value || srcRow["Email"].ToString().Length > 0)
        {
            var email = new Email
            {
                DDDId = (int)srcRow["ID"],
                EmailAddress = srcRow["Email"].ToString(),
                EmailAddressType = ILCDirectory.Data.Enums.EmailAddressType.Personal,
            };
            await repo.InsertEmailAsync(config, email);
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Vehicle
    OleDbCommand vehicleCmd = new OleDbCommand("select * from tblVehicle", connect);
    OleDbDataAdapter daVehicle = new OleDbDataAdapter(vehicleCmd);
    DataSet dsetVehicle = new DataSet();
    daVehicle.Fill(dsetVehicle);

    foreach (DataRow srcRow in dsetVehicle.Tables[0].Rows)
    {
        var vehicle = new Vehicle();
        vehicle.DDDId = (int)srcRow["ID"];
        vehicle.Notes = srcRow["AuditTrail"].ToString();
        vehicle.Color = srcRow["Color"].ToString();
        vehicle.Make = srcRow["Make"].ToString();
        vehicle.Model = srcRow["Model"].ToString();
        vehicle.OwnerPersonId = (int)srcRow["VehicleOwner"];
        vehicle.PermitExpires = srcRow["PermitExpires"] != null ? DateTime.Parse(srcRow["PermitExpires"].ToString()) : null;
        vehicle.PermitNumber = (int)srcRow["PermitNumber"];
        vehicle.PermitType = srcRow["PermitType"].ToString();
        vehicle.ModifiedByUserName = modifiedByUserName;
        vehicle.CreateDateTime = DateTimeOffset.Now;
        vehicle.ModifiedDateTime = DateTimeOffset.Now;
        vehicle.ModifiedByUserName = "Data Migrator";
        await repo.InsertVehicleAsync(config, vehicle);
    }

    connect.Close();
}