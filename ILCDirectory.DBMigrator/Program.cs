using (OleDbConnection connect = new OleDbConnection(
        "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\git\\SIL\\DDD-Tables.accdb;Persist Security Info=False;"))
{
    var configurationBuilder = new ConfigurationBuilder();
    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    configurationBuilder.AddUserSecrets(typeof(Program).GetTypeInfo().Assembly, optional: false);
    IConfigurationRoot config = configurationBuilder.Build();

    // lookup dictionaries
    var titleDictionary = new Dictionary<int, string>();
    var classificationDictionary = new Dictionary<int, Classification>();

    connect.Open();

    // Building
    OleDbCommand cmdBuilding = new OleDbCommand("select * from tblBuildings", connect);
    OleDbDataAdapter daBuilding = new OleDbDataAdapter(cmdBuilding);
    DataSet dsetBuilding = new DataSet();
    daBuilding.Fill(dsetBuilding);
    var buildingRepo = new BuildingRepository(config);
    foreach (DataRow buildingRow in dsetBuilding.Tables[0].Rows)
    {
        var building = new Building()
        {
            BuildingId = (int)buildingRow["BuildingID"],
            BuildingCode = buildingRow["BuildingCode"].ToString(),
            BuildingLongDesc = buildingRow["BuildingLongDesc"].ToString(),
            BuildingShortDesc = buildingRow["BuildingShortDesc"].ToString(),
        };
        await buildingRepo.InsertAsync(building);
    }

    // USCityInfo - create a temporary dictionary to look up city codes and map to city names as we populate addresses
    var cityCodeCity = new Dictionary<string, string>();
    OleDbCommand cmdCityCode = new OleDbCommand("select * from tblCityCodes", connect);
    OleDbDataAdapter daCityCode = new OleDbDataAdapter(cmdCityCode);
    DataSet dsetCityCode = new DataSet();
    daCityCode.Fill(dsetCityCode);
    foreach (DataRow cityCodeRow in dsetCityCode.Tables[0].Rows)
    {
        if (cityCodeRow["CityCode"] == null || cityCodeRow["CityCode"] == "---")
            continue;

        cityCodeCity.Add(cityCodeRow["CityCode"].ToString(), cityCodeRow["CityLongDesc"].ToString());
    }

    // Classification
    OleDbCommand cmdClassification = new OleDbCommand("select * from tblClassification", connect);
    OleDbDataAdapter daClassification = new OleDbDataAdapter(cmdClassification);
    DataSet dsetClassification = new DataSet();
    daClassification.Fill(dsetClassification);
    var classificationRepo = new ClassificationRepository(config);
    foreach (DataRow classificationRow in dsetClassification.Tables[0].Rows)
    {
        var classification = new Classification()
        {
            ClassificationId = (int)classificationRow["ClassificationID"],
            ClassificationCode = (string)classificationRow["StatusCode"],
            Description = (string)classificationRow["StatusDescription"],
            ModifiedDate = DateTime.Now,
            ModifiedByUser = "Data Migrator"
        };
        classificationDictionary.Add(
            (int)classificationRow["ClassificationId"], classification);
        await classificationRepo.InsertAsync(classification);
    }

    // Title
    OleDbCommand cmdTitle = new OleDbCommand("select * from tblTitle", connect);
    OleDbDataAdapter daTitle = new OleDbDataAdapter(cmdTitle);
    DataSet dsetTitle = new DataSet();
    daTitle.Fill(dsetTitle);
    foreach (DataRow titleRow in dsetTitle.Tables[0].Rows)
    {
        titleDictionary.Add((int)titleRow["TitleId"], (string)titleRow["TitleName"]);
    }

    // Person
    OleDbCommand personCmd = new OleDbCommand("select * from Person", connect);
    OleDbDataAdapter daPerson = new OleDbDataAdapter(personCmd);
    DataSet dsetPerson = new DataSet();
    daPerson.Fill(dsetPerson);

    var personRepo = new PersonRepository(config);
    var addressRepo = new AddressRepository(config);
    var officeDetailsRepo = new OfficeDetailsRepository(config);

    foreach(DataRow srcRow in dsetPerson.Tables[0].Rows)
    {
        var person = new Person();
        person.DDDId = (int)srcRow["ID"];
        person.AuditTrail = srcRow["AuditTrail"].ToString();
        person.ClassificationCode = classificationDictionary[(int)srcRow["StatusCode"]].ClassificationCode;
        person.Comment = srcRow["Comment"].ToString();
        person.DateOfBirth = (DateTime?)srcRow["BirthDate"];
        person.DeleteFlag = false;
        person.DirCorrFormNote = srcRow["DirCorrFormNote"].ToString();
        person.DirectoryCorrectionForm = srcRow["DirectoryCorrectionForm"] == null ? null : DateTime.Parse((string)srcRow["DirectoryCorrectionForm"]);
        person.Title = srcRow["Title"] == null ? "" : titleDictionary[(int)srcRow["Title"]];
        person.FirstName = srcRow["FirstName"].ToString();
        person.MiddleName = srcRow["MiddleName"].ToString();
        person.LastName = srcRow["LastName"].ToString();
        person.NickName = srcRow["NickName"].ToString();
        person.MaidenName = srcRow["MaidenName"].ToString();
        person.Suffix = srcRow["Suffix"].ToString();
        person.Gender = srcRow["Gender"].ToString();
        person.LanguagesSpoken = srcRow["LanguagesSpoken"].ToString();
        person.MaritalStatus = srcRow["MaritalStatus"].ToString();
        person.ModifiedByUserName = "ILCDirectoryMigrator";
        person.CreateDateTime = DateTimeOffset.Now;
        person.ModifiedDateTime = DateTimeOffset.Now;
        await personRepo.InsertAsync(person);

        // pull out directory address info
        var address = new Address();
        address.AddressLine1 = srcRow["DirectoryAddress"].ToString();
        address.City = srcRow["DirectoryCity"].ToString();
        address.ZipCode = srcRow["DirectoryZIP"].ToString();
        
        address.IsActive = true;                                    // TODO: which way to default?
        address.IsVerified = true;

        await addressRepo.InsertAsync(address);

        // pull out Office Info
        var officeDetails = new OfficeDetails();
        officeDetails.BuildingId = (int?)srcRow["BuildingCode"];
    }

    // Address
    OleDbCommand addressCmd = new OleDbCommand("select * from Addresses", connect);
    OleDbDataAdapter daAddress = new OleDbDataAdapter(addressCmd);
    DataSet dsetAddress = new DataSet();
    daAddress.Fill(dsetAddress);

    foreach (DataRow srcRow in dsetAddress.Tables[0].Rows)
    {
        var address = new Address();
        address.DDDId = (int)srcRow["ID"];
        address.AuditTrail = srcRow["AuditTrail"].ToString();
        address.AddressLine1 = srcRow["AddressLine1"].ToString();
        address.AddressLine2 = srcRow["AddressLine2"].ToString();
        address.AddressLine3 = srcRow["AddressLine3"].ToString();
        address.AddressLine4 = srcRow["AddressLine4"].ToString();
        address.City = srcRow["City"].ToString();
        address.ContactPersonId = (int)srcRow["ContactPersonId"];

        address.IncludeInDirectory = (bool)srcRow["IncludeInDirectory"];
        address.CreateDateTime = DateTimeOffset.Now;
        address.ModifiedDateTime = DateTimeOffset.Now;
        await addressRepo.InsertAsync(address);
    }

    // Vehicle
    OleDbCommand vehicleCmd = new OleDbCommand("select * from tblVehicle", connect);
    OleDbDataAdapter daVehicle = new OleDbDataAdapter(vehicleCmd);
    DataSet dsetVehicle = new DataSet();
    daVehicle.Fill(dsetVehicle);

    var vehicleRepo = new VehicleRepository(config);
    foreach (DataRow srcRow in dsetVehicle.Tables[0].Rows)
    {
        var vehicle = new Vehicle();
        vehicle.DDDId = (int)srcRow["ID"];
        vehicle.AuditTrail = srcRow["AuditTrail"].ToString();
        vehicle.Color = srcRow["Color"].ToString();
        vehicle.Make = srcRow["Make"].ToString();
        vehicle.Model = srcRow["Model"].ToString();
        vehicle.OwnerPersonId = (int)srcRow["VehicleOwner"];
        vehicle.PermitExpires = srcRow["PermitExpires"] != null ? DateTime.Parse(srcRow["PermitExpires"].ToString()) : null;
        vehicle.PermitNumber = (int)srcRow["PermitNumber"];
        vehicle.PermitType = srcRow["PermitType"].ToString();
        vehicle.ModifiedByUserName = "ILCDirectoryMigrator";
        vehicle.CreateDateTime = DateTimeOffset.Now;
        vehicle.ModifiedDateTime = DateTimeOffset.Now;
        await vehicleRepo.InsertAsync(vehicle);
    }

    // select TOP 1000 * from person p inner join Addresses a ON a.ID = p.ID ORDER BY p.ID DESC


    connect.Close();
}