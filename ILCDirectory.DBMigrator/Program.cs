﻿using ILCDirectory.Data.Enums;
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

var configurationBuilder = new ConfigurationBuilder();
configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
configurationBuilder.AddUserSecrets(typeof(Program).GetTypeInfo().Assembly, optional: false);
IConfigurationRoot config = configurationBuilder.Build();
var modifiedByUserName = "ILCDirectoryMigrator";
DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", SqlClientFactory.Instance);

await doMigration(config);
var tokenizeAndSearchRepository = new TokenizeAndSearchRepository();
await tokenizeAndSearchRepository.PopulateSearchTokenTables(config);
var (persons, addresses) = await tokenizeAndSearchRepository.SearchForPersonOrAddress(config, "Caleb");
foreach (var person in persons)
{
    Console.WriteLine($"{person.LastName} ({person.MaidenName}), {person.FirstName} {person.MiddleName} ({person.NickName})");
}
foreach (var address in addresses)
{
    Console.WriteLine($"{address.AddressLine1}, {address.AddressLine2}, {address.City}, {address.StateProvince} {address.PostalCode}");
}   

async Task doMigration(IConfiguration config)
{
    using (OleDbConnection connect = new OleDbConnection(
            "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=F:\\Dropbox\\ILC\\DDD-FinalExport.accdb;Persist Security Info=False;"))
    {
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
        var allBuildingRows = await repo.GetAllRowsAsync<Building>(config, "Building");
        foreach (DataRow buildingRow in dsetBuilding.Tables[0].Rows)
        {
            var building = new Building()
            {
                BuildingId = (int)buildingRow["BuildingID"],
                BuildingCode = buildingRow["BuildingCode"].ToString().Trim(),
                BuildingLongDesc = buildingRow["BuildingLongDesc"].ToString().Trim(),
                BuildingShortDesc = buildingRow["BuildingShortDesc"].ToString().Trim(),
                ModifiedByUserName = "Data Migrator",
            };
            if (!allBuildingRows.Any(b => b.BuildingId == building.BuildingId))
                await repo.InsertBuildingAsync(config, building, true);
        }
        allBuildingRows = await repo.GetAllRowsAsync<Building>(config, "Building"); // refresh list of buildings
        Console.WriteLine($"Building rows inserted: {allBuildingRows.Count}");

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // USCityInfo - create a temporary dictionary to look up city codes and map to city names as we populate addresses
        var cityCodeCity = new Dictionary<string, string>();
        OleDbCommand cmdCityCode = new OleDbCommand("select * from tblCityCodes", connect);
        OleDbDataAdapter daCityCode = new OleDbDataAdapter(cmdCityCode);
        DataSet dsetCityCode = new DataSet();
        daCityCode.Fill(dsetCityCode);
        foreach (DataRow cityCodeRow in dsetCityCode.Tables[0].Rows)
        {
            var cityCode = cityCodeRow["CityCode"];

            if (cityCode == DBNull.Value || cityCodeRow["CityCode"].ToString() == "---")
                continue;

            var cityCodeDesc = cityCodeRow["CityLongDesc"];

            if (cityCodeDesc == DBNull.Value)
                continue;

            cityCode = cityCode.ToString().Trim();
            cityCodeDesc = cityCodeDesc.ToString().Trim();

            if (!cityCodeCity.ContainsKey((string)cityCode))
                cityCodeCity.Add((string)cityCode, (string)cityCodeDesc);
        }
        Console.WriteLine($"CityCodeCity rows inserted: {cityCodeCity.Count}");

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Classification
        OleDbCommand cmdClassification = new OleDbCommand("select * from tblClassification", connect);
        OleDbDataAdapter daClassification = new OleDbDataAdapter(cmdClassification);
        DataSet dsetClassification = new DataSet();
        daClassification.Fill(dsetClassification);
        var allCurrentClassificationRows = await repo.GetAllRowsAsync<Classification>(config, "Classification");
        foreach (DataRow classificationRow in dsetClassification.Tables[0].Rows)
        {
            if (classificationRow["StatusCode"] == DBNull.Value)
                continue;

            var classification = new Classification()
            {
                ClassificationId = (int)classificationRow["ClassificationID"],
                ClassificationCode = classificationRow["StatusCode"].ToString().Trim(),
                Description = classificationRow["StatusDescription"].ToString().Trim(),
                ModifiedDateTime = DateTime.Now,
                ModifiedByUserName = modifiedByUserName
            };

            if (!classificationDictionary.ContainsKey((int)classificationRow["ClassificationId"]))
                classificationDictionary.Add((int)classificationRow["ClassificationId"], classification);

            if (!allCurrentClassificationRows.Any(c => c.ClassificationId == classification.ClassificationId))
                await repo.InsertClassificationAsync(config, classification, true);
        }
        allCurrentClassificationRows = await repo.GetAllRowsAsync<Classification>(config, "Classification"); // refresh list of classifications
        Console.WriteLine($"Classification rows inserted: {allCurrentClassificationRows.Count}");

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // DeliveryCodeDescription
        OleDbCommand cmdDeliveryCodeLocation = new OleDbCommand("select * from tblMailDelivery", connect);
        OleDbDataAdapter daDeliveryCodeLocation = new OleDbDataAdapter(cmdDeliveryCodeLocation);
        DataSet dsetDeliveryCodeLocation = new DataSet();
        daDeliveryCodeLocation.Fill(dsetDeliveryCodeLocation);
        var allCurrentDeliveryCodeLocationRows = await repo.GetAllRowsAsync<DeliveryCodeLocation>(config, "DeliveryCodeLocation");
        // this table contains duplicate delivery codes, so we need to combine the delivery location values into a single string
        // and then insert a single row for each delivery code
        Dictionary<string, string> deliveryCodeLocation = new();
        foreach (DataRow row in dsetDeliveryCodeLocation.Tables[0].Rows)
        {
            if (row["DeliveryCode"] == DBNull.Value)
                continue;

            if (!deliveryCodeLocation.ContainsKey(row["DeliveryCode"].ToString().Trim()))
                deliveryCodeLocation.Add(row["DeliveryCode"].ToString().Trim(), row["DeliveryLocation"].ToString().Trim().Trim());
            else
                deliveryCodeLocation[row["DeliveryCode"].ToString().Trim()] += ", " + row["DeliveryLocation"].ToString().Trim();
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

            await repo.InsertDeliveryCodeLocationAsync<DeliveryCodeLocation>(config, rowdata);
        }
        allCurrentDeliveryCodeLocationRows = await repo.GetAllRowsAsync<DeliveryCodeLocation>(config, "DeliveryCodeLocation"); // refresh list of delivery codes
        Console.WriteLine($"DeliveryCodeLocation rows inserted: {allCurrentDeliveryCodeLocationRows.Count}");

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
                titleDictionary.Add((int)titleRow["TitleId"], titleRow["TitleName"].ToString().Trim());
        }
        Console.WriteLine($"Title rows inserted: {titleDictionary.Count}");

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // Person
        OleDbCommand personCmd = new OleDbCommand("select * from Person", connect);
        OleDbDataAdapter daPerson = new OleDbDataAdapter(personCmd);
        DataSet dsetPerson = new DataSet();
        daPerson.Fill(dsetPerson);

        var allPersonRows = await repo.GetAllRowsAsync<Person>(config, "Person");
        var allParentChildRows = await repo.GetAllRowsAsync<ParentChild>(config, "ParentChild");
        var allAddressRows = await repo.GetAllRowsAsync<Address>(config, "Address");
        var allInternalAddressRows = await repo.GetAllRowsAsync<InternalAddress>(config, "InternalAddress");
        var allOfficeDetailsRows = await repo.GetAllRowsAsync<OfficeDetails>(config, "OfficeDetails");
        var allHouseholdRows = await repo.GetAllRowsAsync<Household>(config, "Household");
        var allPersonHouseholdRows = await repo.GetAllRowsAsync<PersonHousehold>(config, "PersonHousehold");
        var allHouseholdAddressRows = await repo.GetAllRowsAsync<HouseholdAddress>(config, "HouseholdAddress");
        var allEmailRows = await repo.GetAllRowsAsync<Email>(config, "Email");

        foreach (DataRow srcRow in dsetPerson.Tables[0].Rows)
        {
            if ((bool)srcRow["DeleteFlag"] == true)
                continue;

            var person = new Person();
            person.DDDId = (int)srcRow["ID"];
            person.PersonId = (int)srcRow["ID"];
            person.Notes = srcRow["AuditTrail"].ToString().Trim();
            if (srcRow["StatusCode"] == DBNull.Value || Convert.ToInt32((string)srcRow["StatusCode"]) == 26)
                person.ClassificationCode = null; // This was a row with all nulls except the id in the source data
            else
                person.ClassificationCode = classificationDictionary[Convert.ToInt32((string)srcRow["StatusCode"])].ClassificationCode;
            person.Comment = srcRow["Comment"].ToString().Trim();
            person.DateOfBirth = srcRow["BirthDate"] == DBNull.Value ? null : (DateTime)srcRow["BirthDate"];
            person.DeleteFlag = false;
            person.DirCorrFormNote = srcRow["DirCorrFormNote"] == DBNull.Value ? null : srcRow["DirCorrFormNote"].ToString();
            person.DirectoryCorrectionForm = srcRow["DirectoryCorrectionForm"] == DBNull.Value ? null : (DateTime)srcRow["DirectoryCorrectionForm"];
            person.Title = srcRow["Title"] == DBNull.Value ? null : (Title)Convert.ToInt32(srcRow["Title"].ToString());
            person.FirstName = srcRow["FirstName"].ToString().Trim();
            person.MiddleName = srcRow["MiddleName"] == DBNull.Value ? null : srcRow["MiddleName"].ToString().Trim();
            person.LastName = srcRow["LastName"].ToString().Trim();

            // some rows have a null for firstname and lastname, and it indicates an unused row. We'll skip those
            if (string.IsNullOrEmpty(person.FirstName) && string.IsNullOrEmpty(person.LastName))
                continue;

            person.NickName = srcRow["NickName"] == DBNull.Value ? null : srcRow["NickName"].ToString().Trim();
            person.NickName = (person.NickName == person.FirstName) ? null : person.NickName; // only set it if different from first name

            person.MaidenName = srcRow["MaidenName"] == DBNull.Value ? null : srcRow["MaidenName"].ToString().Trim();
            person.Suffix = srcRow["Suffix"] == DBNull.Value ? null : (Suffix)Convert.ToInt32(srcRow["Suffix"].ToString().Trim());
            person.Gender = srcRow["Gender"].ToString().Trim();
            person.LanguagesSpoken = srcRow["LanguagesSpoken"] == DBNull.Value ? null : srcRow["LanguagesSpoken"].ToString().Trim();
            person.MaritalStatus = srcRow["MaritalStatus"] == DBNull.Value ? null : srcRow["MaritalStatus"].ToString().Trim();
            person.Position = srcRow["Position"] == DBNull.Value ? null : srcRow["Position"].ToString().Trim();
            person.WoCode = srcRow["WO_Code"] == DBNull.Value ? null : srcRow["WO_Code"].ToString().Trim();
            person.WorkgroupCode = srcRow["WorkgroupCode"] == DBNull.Value ? null : (int)srcRow["WorkgroupCode"];
            person.IncludeInDirectory = (bool)srcRow["DirectoryInclude"];

            person.ModifiedByUserName = modifiedByUserName;
            person.CreateDateTime = DateTimeOffset.Now;
            person.ModifiedDateTime = DateTimeOffset.Now;

            if (person.LanguagesSpoken != null && person.LanguagesSpoken.ToLower().Contains("deceased"))
                continue;

            if (!allPersonRows.Any(p => p.DDDId == person.DDDId))
            {
                person = await repo.InsertPersonAsync(config, person, true);
                allPersonRows.Add(person);
            }

            var personHousehold = allPersonHouseholdRows.FirstOrDefault(x => x.PersonId == person.PersonId);
            Household? household = personHousehold == null ? null : allHouseholdRows.FirstOrDefault(x => x.HouseholdId == personHousehold.HouseholdId);

            // Check if person has spouse or children, and if they currently exist (thus, have a household row)
            if (srcRow["SpouseNameID"] != DBNull.Value)
            {
                var spouseDDDId = (int)srcRow["SpouseNameID"];

                // person has spouse, check if spouse exists in current data
                // if spouse DDDId is less than current person DDDId, then we already processed the spouse, thus we have created a household row
                if (spouseDDDId < person.DDDId)
                {
                    var spouse = allPersonRows.FirstOrDefault(p => p.DDDId == spouseDDDId);
                    var spousePersonHousehold = allPersonHouseholdRows.FirstOrDefault(x => x.PersonId == spouse?.PersonId);
                    if (spousePersonHousehold != null)
                        household = allHouseholdRows.Where(y => y.HouseholdId == spousePersonHousehold.HouseholdId).FirstOrDefault();
                }
            }

            if (household == null) // need to create a household row
            {
                household = new Household();
                household.HouseholdName = srcRow["LastName"].ToString();
                household.ModifiedByUserName = modifiedByUserName;
                household.CreateDateTime = DateTimeOffset.Now;
                household.ModifiedDateTime = DateTimeOffset.Now;
                household = await repo.InsertRowAsync<Household>(config, household, "Household");
                allHouseholdRows.Add(household);
            }

            // now we have person and household, so we can create the relationship if it doesn't already exist
            if (!allPersonHouseholdRows.Any(x => x.PersonId == person.PersonId && x.HouseholdId == household.HouseholdId))
            {
                var personHoushold = new PersonHousehold();
                personHoushold.HouseholdId = (int)household.HouseholdId;
                personHoushold.PersonId = (int)person.PersonId;
                personHoushold.ModifiedByUserName = modifiedByUserName;
                personHoushold.CreateDateTime = DateTimeOffset.Now;
                personHoushold.ModifiedDateTime = DateTimeOffset.Now;
                await repo.InsertRowAsync<PersonHousehold>(config, personHoushold, "PersonHousehold");
                allPersonHouseholdRows.Add(personHoushold);
            }

            Address address = CreateAndCalculateAddress((int)srcRow["ID"], null,
                srcRow["DirectoryAddress"] == DBNull.Value ? null : srcRow["DirectoryAddress"]?.ToString()?.Trim(),
                null,
                null,
                srcRow["DirectoryZip"]?.ToString()?.Trim(),
                srcRow["DirectoryCity"] == DBNull.Value ? null : srcRow["DirectoryCity"]?.ToString()?.Trim(),
                null,
                null,
                (bool)srcRow["DirectoryInclude"],                
                cityCodeCity, null, null,
                modifiedByUserName, ref allAddressRows, ref allPersonHouseholdRows, ref allHouseholdAddressRows);

            Address existingOrNew = null;

            if (address != null) // if null, it's already been logged, or it's a cancelled visit
            {
                existingOrNew = allAddressRows.FirstOrDefault(a => a.AddressLine1 == address.AddressLine1 && a.City == address.City && a.PostalCode == address.PostalCode);
                if (existingOrNew == null && !string.IsNullOrEmpty(address.AddressLine1) && !string.IsNullOrEmpty(address.City) && !string.IsNullOrEmpty(address.StateProvince) && !string.IsNullOrEmpty(address.PostalCode))
                {
                    address.ModifiedByUserName = modifiedByUserName;
                    address.CreateDateTime = DateTimeOffset.Now;
                    address.ModifiedDateTime = DateTimeOffset.Now;

                    if (address.City == null)
                        Console.WriteLine();

                    existingOrNew = await repo.InsertRowAsync<Address>(config, address, "Address");
                    allAddressRows.Add(existingOrNew);
                }
            }

            // we'll create a InternalAddress row for each person
            InternalAddress internalAddress = new InternalAddress()
            {
                PersonId = person.PersonId.Value,
                RoomNumber = srcRow["DirectoryRoom"].ToString(),
                MailListFlag = (bool)srcRow["MailListFlag"],
                MailOnly = (bool)srcRow["MailOnly"],
                MailSortName = srcRow["MailSortName"].ToString(),
                SpecialContactInfo = srcRow["SpecialContactInfo"].ToString()
            };

            if (!allInternalAddressRows.Any(x => x.PersonId == person.PersonId))
            {
                // write the InternalAddress row
                await repo.InsertRowAsync<InternalAddress>(config, internalAddress, "InternalAddress");
                allInternalAddressRows.Add(internalAddress);
            }

            // now associate the address with the person via the HouseholdAddress table if it doesn't already exist
            if (existingOrNew != null && !allHouseholdAddressRows.Any(x => x.HouseholdId == household.HouseholdId && x.AddressId == existingOrNew.AddressId))
            {
                var householdAddress = new HouseholdAddress();
                householdAddress.HouseholdId = household.HouseholdId.Value;
                householdAddress.AddressId = existingOrNew.AddressId.Value;
                householdAddress.ModifiedByUserName = modifiedByUserName;
                householdAddress.CreateDateTime = DateTimeOffset.Now;
                householdAddress.ModifiedDateTime = DateTimeOffset.Now;
                await repo.InsertRowAsync<HouseholdAddress>(config, householdAddress, "HouseholdAddress");
                allHouseholdAddressRows.Add(householdAddress);
            }

            address = null; // make sure this is not used below

            // create OfficeDetails if they are present in the source data
            if (srcRow["BuildingCode"] != DBNull.Value || srcRow["CubicleNumber"] != DBNull.Value || srcRow["RoomNumber"] != DBNull.Value)
            {
                //int? buildingId = srcRow["BuildingCode"] == DBNull.Value ? null : Convert.ToInt32(srcRow["BuildingCode"].ToString()); // ahh that it could be this simple
                // some of the building rows in the source refer to a building code, others refer to a building id
                int? buildingId = allBuildingRows.FirstOrDefault(x => x.BuildingCode == srcRow["BuildingCode"].ToString().Trim() || x.BuildingId.ToString() == srcRow["BuildingCode"].ToString().Trim())?.BuildingId;

                var cubicleNumber = srcRow["CubicleNumber"] == DBNull.Value ? null : srcRow["CubicleNumber"].ToString().Trim();
                var roomNumber = srcRow["RoomNumber"] == DBNull.Value ? null : srcRow["RoomNumber"].ToString().Trim();

                if (!allOfficeDetailsRows.Any(x => x.PersonId == person.PersonId))
                {
                    var officeDetails = new OfficeDetails();

                    officeDetails.DDDPersonId = (int)srcRow["ID"];
                    officeDetails.PersonId = (int)person.PersonId;
                    officeDetails.BuildingId = buildingId;
                    officeDetails.CubicleNumber = cubicleNumber;
                    officeDetails.RoomNumber = roomNumber;
                    officeDetails.ModifiedByUserName = modifiedByUserName;
                    officeDetails.CreateDateTime = DateTimeOffset.Now;
                    officeDetails.ModifiedDateTime = DateTimeOffset.Now;
                    await repo.InsertRowAsync<OfficeDetails>(config, officeDetails, "OfficeDetails");
                    allOfficeDetailsRows.Add(officeDetails);
                }
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Address
            // addr repo config'd above
            // so -- the address table also contains email addresses, home phone, cell phone, contact person and notes which
            // include departure instructions that should have just been set in the visit table. I'm thinking we'll be forced
            // to ignore the notes, but set the phone numbers and email addresses.
            // Getting just the addresses tied to this person
            OleDbCommand addressCmd = new OleDbCommand($"select * from Addresses where ID = {person.DDDId}", connect);
            OleDbDataAdapter daAddress = new OleDbDataAdapter(addressCmd);
            DataSet dsetAddress = new DataSet();
            daAddress.Fill(dsetAddress);

            foreach (DataRow srcAddressRow in dsetAddress.Tables[0].Rows)
            {
                Address address2 = CreateAndCalculateAddress((int)srcAddressRow["ID"], srcAddressRow["AuditTrail"]?.ToString()?.Trim(),
                    srcAddressRow["AddressLine1"] == DBNull.Value ? null : srcAddressRow["AddressLine1"]?.ToString()?.Trim(),
                    srcAddressRow["AddressLine2"] == DBNull.Value ? null : srcAddressRow["AddressLine2"]?.ToString()?.Trim(),
                    srcAddressRow["State"] == DBNull.Value ? null : srcAddressRow["State"]?.ToString()?.Trim(),
                    srcAddressRow["ZipCode"] == DBNull.Value ? null : srcAddressRow["ZipCode"]?.ToString()?.Trim(),
                    srcAddressRow["CityCode"] == DBNull.Value ? null : srcAddressRow["CityCode"]?.ToString()?.Trim(),
                    srcAddressRow["ContactPerson"] == DBNull.Value ? null : srcAddressRow["ContactPerson"]?.ToString()?.Trim(),
                    srcAddressRow["ContactPhone"] == DBNull.Value ? null : srcAddressRow["ContactPhone"]?.ToString()?.Trim(),
                    (bool)srcRow["DirectoryInclude"],
                    cityCodeCity, srcAddressRow["ArrivalDate"] == DBNull.Value ? null : new DateTimeOffset((DateTime)srcAddressRow["ArrivalDate"]), 
                    srcAddressRow["DepartureDate"] == DBNull.Value ? null : new DateTimeOffset((DateTime)srcAddressRow["DepartureDate"]),
                    modifiedByUserName, ref allAddressRows, ref allPersonHouseholdRows, ref allHouseholdAddressRows);

                //if (address2 != null && (address2.AddressLine1 == null || address2.City == null))
                //    Debug.Assert(false);

                // two or more people not in the same household can be at the same address, so we need to check if the address already exists for this household
                // find any existing address rows that match the address we just created
                if (address2 != null)
                {
                    var existingAddresses = allAddressRows.Where(a => a.AddressLine1 == address2.AddressLine1 && a.City == address2.City && a.PostalCode == address2.PostalCode);
                    // now find the householdAddess rows that match the address rows
                    var existingHouseholdAddresses = allHouseholdAddressRows.Where(h => existingAddresses.Any(a => a.AddressId == h.AddressId));
                    // this person's household has been determined, so check that household matches one of the existing households
                    if (!existingHouseholdAddresses.Any(h => h.HouseholdId == household.HouseholdId))
                    {
                        // no household address for this address. Add the address and the householdAddress rows
                        address2.ModifiedByUserName = modifiedByUserName;
                        address2.CreateDateTime = DateTimeOffset.Now;
                        address2.ModifiedDateTime = DateTimeOffset.Now;

                        if (address2.City == null)
                            Console.WriteLine();

                        address2 = await repo.InsertRowAsync<Address>(config, address2, "Address");
                        allAddressRows.Add(address2);

                        var householdAddress = new HouseholdAddress();
                        householdAddress.HouseholdId = household.HouseholdId.Value;
                        householdAddress.AddressId = address2.AddressId.Value;
                        householdAddress.ModifiedByUserName = modifiedByUserName;
                        householdAddress.CreateDateTime = DateTimeOffset.Now;
                        householdAddress.ModifiedDateTime = DateTimeOffset.Now;
                        await repo.InsertRowAsync<HouseholdAddress>(config, householdAddress, "HouseholdAddress");
                        allHouseholdAddressRows.Add(householdAddress);
                    }
                }

                // Email
                if (srcAddressRow["Email"] != DBNull.Value && srcAddressRow["Email"].ToString().Trim().Length > 0)
                {
                    // some email columns are multiple email addresses separated by a semicolon, or words, so we'll parse them out and throw out any that don't look like email addresses
                    Regex regexValidEmail = new Regex(@"[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}");
                    string?[] emailAddresses = srcAddressRow["Email"]?.ToString().Trim().Split(new char[] { ';', ' ' });
                    // put emails found in a list, cause there are some duplicates
                    List<string> emailList = new List<string>();
                    foreach (string? emailAddress in emailAddresses)
                    {
                        if (emailAddress == null)
                            continue;

                        var emailAddressTrim = emailAddress.Trim();

                        if (!regexValidEmail.IsMatch(emailAddressTrim))
                            continue;

                        if (!emailList.Contains(emailAddressTrim))
                            emailList.Add(emailAddressTrim);
                    }
                    foreach (string emailAddress in emailList)
                    {
                        var email = new Email();
                        email.PersonId = (int)person.PersonId;
                        email.EmailAddressType = EmailAddressType.Personal;
                        email.DDDId = (int)srcAddressRow["ID"];
                        email.EmailAddress = emailAddress;
                        email.ModifiedByUserName = modifiedByUserName;
                        email.CreateDateTime = DateTimeOffset.Now;
                        email.ModifiedDateTime = DateTimeOffset.Now;
                        await repo.InsertRowAsync<Email>(config, email, "Email");
                        allEmailRows.Add(email);
                    }
                }
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Vehicle
            OleDbCommand vehicleCmd = new OleDbCommand($"select * from tblVehicle where VehicleOwner = {person.DDDId}", connect);

            OleDbDataAdapter daVehicle = new OleDbDataAdapter(vehicleCmd);
            DataSet dsetVehicle = new DataSet();
            daVehicle.Fill(dsetVehicle);

            foreach (DataRow srcVehicleRow in dsetVehicle.Tables[0].Rows)
            {
                // we may want to omit vehicles that have a permit expiration date before today and a permit type of "temp" and vehicle detals that are null
                //if (srcVehicleRow[1"PermitExpires"] != DBNull.Value && (DateTime)srcVehicleRow["PermitExpires"] < DateTime.Now && srcVehicleRow["PermitType"].ToString() == "temp")
                // we should make a list of all expired permits and make a notification to the user that they need to be updated

                // some rows have year, color make and model all DbNull values - skip those
                if (srcVehicleRow["VehicleYear"] == DBNull.Value && srcVehicleRow["VehicleColor"] == DBNull.Value && srcVehicleRow["VehicleMake"] == DBNull.Value && srcVehicleRow["VehicleModel"] == DBNull.Value)
                    continue;

                var vehicle = new Vehicle();
                vehicle.VehicleOwner = (int)srcVehicleRow["VehicleOwner"];
                vehicle.DDDId = (int)srcVehicleRow["VehicleOwner"];
                //vehicle.Notes = srcVehicleRow["AuditTrail"].ToString(); // no need for this data - it is the person and the one who did data entry
                vehicle.Color = srcVehicleRow["VehicleColor"].ToString().Trim();
                vehicle.Make = srcVehicleRow["VehicleMake"].ToString().Trim();
                vehicle.Model = srcVehicleRow["VehicleModel"].ToString().Trim();
                vehicle.Year = Convert.ToInt32(srcVehicleRow["VehicleYear"] == DBNull.Value ? null : srcVehicleRow["VehicleYear"].ToString().Trim());
                vehicle.PermitExpires = srcVehicleRow["PermitExpires"] != DBNull.Value ? (DateTime)srcVehicleRow["PermitExpires"] : null;
                vehicle.PermitNumber = srcVehicleRow["PermitNumber"] != DBNull.Value ? (int)srcVehicleRow["PermitNumber"] : null;
                vehicle.PermitType = srcVehicleRow["PermitType"].ToString().Trim();
                vehicle.ModifiedByUserName = modifiedByUserName;
                vehicle.CreateDateTime = DateTimeOffset.Now;
                vehicle.ModifiedDateTime = DateTimeOffset.Now;
                vehicle.ModifiedByUserName = "Data Migrator";
                await repo.InsertRowAsync<Vehicle>(config, vehicle, "Vehicle");
            }

            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            // Children
            OleDbCommand childrenCmd = new OleDbCommand($"select * from Children where FatherID = {person.DDDId} OR MotherID = {person.DDDId}", connect);

            OleDbDataAdapter daChildren = new OleDbDataAdapter(childrenCmd);
            DataSet dsetChildren = new DataSet();
            daChildren.Fill(dsetChildren);

            foreach (DataRow row in dsetChildren.Tables[0].Rows)
            {
                // load in and associate child rows into Person table but only if the child is not already in the Person table.
                // Add ParentChild record for each parent child relationship if they don't already exist
                var child = new Person();
                child.DDDId = (int)row["ChildID"] + 20000; // because these are coming from a different table, the ids could collide with existing person ids
                child.PersonId = (int)row["ChildID"] + 20000;

                if (!allPersonRows.Any(p => p.DDDId == child.DDDId))
                {
                    child.Notes = row["AuditTrail"] == DBNull.Value ? null : row["AuditTrail"].ToString().Trim();
                    child.ClassificationCode = null;
                    child.Comment = null;
                    child.FirstName = row["ChildName"] == DBNull.Value ? null : row["ChildName"].ToString().Trim();
                    if (child.FirstName == null)
                        continue;
                    child.LastName = row["ChildName"] == DBNull.Value ? null : 
                        string.IsNullOrWhiteSpace(row["ChildLastName"].ToString()) ? person.LastName : row["ChildLastName"].ToString().Trim();
                    child.Gender = row["ChildGender"].ToString().Trim();

                    if (row["ChildBirthdate"] != DBNull.Value)
                    {
                        child.DateOfBirth = (DateTime)row["ChildBirthdate"];
                    }
                    else
                    {
                        // create a regex that captures the DateTime from within a string where the date is in the middle of the string
                        Regex regexDateTime = new Regex(@"\d{1,2}/\d{1,2}/\d{4}");
                        // Now extract the datetime from the string - the child is at least as old as this entry date
                        if (child.Notes != null)
                        {
                            var match = regexDateTime.Match(child.Notes.Split("\n")[0]);
                            if (match.Success)
                            {
                                child.DateOfBirth = Convert.ToDateTime(match.Value);
                                child.Notes = "Birthdate empty on migration - used last audit entry date - child is at least that age" + Environment.NewLine +
                                    child.Notes;
                            }
                        }

                    }

                    child.DeleteFlag = false;
                    child.DirCorrFormNote = null;
                    child.DirectoryCorrectionForm = null;
                    child.Title = null;
                    child.MiddleName = null;
                    child.NickName = null;
                    child.MaidenName = null;
                    child.Suffix = null;
                    child.LanguagesSpoken = null;
                    child.MaritalStatus = null;
                    child.Position = null;
                    child.WoCode = null;
                    child.WorkgroupCode = null;
                    child.IncludeInDirectory = false;
                    child.ModifiedByUserName = modifiedByUserName;
                    child.CreateDateTime = DateTimeOffset.Now;
                    child.ModifiedDateTime = DateTimeOffset.Now;

                    child = await repo.InsertPersonAsync(config, child, true);
                    allPersonRows.Add(child);

                    // also insert personHousehold row
                    var personHoushold = new PersonHousehold();
                    personHoushold.HouseholdId = (int)household.HouseholdId;
                    personHoushold.PersonId = (int)child.PersonId;
                    personHoushold.ModifiedByUserName = modifiedByUserName;
                    personHoushold.CreateDateTime = DateTimeOffset.Now;
                    personHoushold.ModifiedDateTime = DateTimeOffset.Now;
                    await repo.InsertRowAsync<PersonHousehold>(config, personHoushold, "PersonHousehold");
                    allPersonHouseholdRows.Add(personHoushold);
                }

                // now add ParentChild row if it doesn't already exist
                if (!allParentChildRows.Any(x => x.ParentId == person.PersonId && x.ChildId == child.PersonId))
                {
                    var parentChild = new ParentChild();
                    parentChild.ParentId = (int)person.PersonId;
                    parentChild.ChildId = (int)child.PersonId;
                    parentChild.ModifiedByUserName = modifiedByUserName;
                    parentChild.CreateDateTime = DateTimeOffset.Now;
                    parentChild.ModifiedDateTime = DateTimeOffset.Now;
                    await repo.InsertRowAsync<ParentChild>(config, parentChild, "ParentChild");
                    allParentChildRows.Add(parentChild);
                }
            }
            if (allPersonRows.Count % 10 == 0)
                Console.Write(".");
            if (allPersonRows.Count % 100 == 0)
                Console.Write("." + Environment.NewLine);
        }
        allPersonRows = await repo.GetAllRowsAsync<Person>(config, "Person"); // refresh list of people
        Console.WriteLine(Environment.NewLine + $"Person rows inserted: {allPersonRows.Count}");

        connect.Close();
    }
}

Address CreateAndCalculateAddress(int id, string? auditTrail, string? addressLine1, string? addressLine2, string? state, string? zipCode, string? cityCode, 
    string? contactPerson, string? contactPhone, bool directoryInclude, Dictionary<string, string> cityCodeCity, DateTimeOffset? arrivalDate, 
    DateTimeOffset? departureDate, string modifiedByUserName,
    ref IList<Address> allAddressRows, ref IList<PersonHousehold> allPersonHouseholdRows, ref IList<HouseholdAddress> allHouseholdAddressRows)
{
    var address = new Address();
    address.DDDId = id;
    address.Notes = auditTrail;
    address.AddressLine1 = string.IsNullOrWhiteSpace(addressLine1) ? null : addressLine1;
    address.AddressLine2 = string.IsNullOrWhiteSpace(addressLine2) ? null : addressLine2;
    address.ArrivalDate = arrivalDate;
    address.DepartureDate = departureDate;
   
    if (departureDate == null) // permanent address
        address.IsPermanent = true;
    else 
        address.IsPermanent = false;

    address.StateProvince = string.IsNullOrWhiteSpace(state) ? null : state;
    address.PostalCode = string.IsNullOrWhiteSpace(zipCode) ? null : zipCode;

    if (cityCode == "")
        cityCode = null;

    if (address.AddressLine1 == null && address.AddressLine2 == null && address.StateProvince == null && address.PostalCode == null && cityCode == null)
        return null; // no address data, so return null

    if (cityCode != null)
    {
        address.City = cityCodeCity.ContainsKey(cityCode) ? cityCodeCity[cityCode] : cityCode.Trim(); // it's called cityCode, but sometimes it's a city name
    }

    if (address?.AddressLine1 != null && address.AddressLine1.ToLower().Contains("cancel"))
    {
        return null;
    }

    // mobile home or guest house address on campus - AddressLine2
    if (address.AddressLine2 != null && (address.AddressLine2.StartsWith("MH") || address.AddressLine2.StartsWith("Mobile Home") || address.AddressLine2.StartsWith("GH") ||
        address.AddressLine2.StartsWith("Guest House") || address.AddressLine2.StartsWith("RV") || address.AddressLine2.StartsWith("MH") ||
        address.AddressLine2.StartsWith("Guesthouse")))
    {
        // added check for fat-fingered postal code 75136 to cover three addresses internal to ILC (2 RV, 1 Guest House)
        if ((string.IsNullOrEmpty(address.PostalCode) || address.PostalCode == "75236" || address.PostalCode == "75136") && (string.IsNullOrEmpty(address.City) || address.City == "Dallas") &&
            (string.IsNullOrEmpty(address.StateProvince) || address.StateProvince == "TX"))
        {
            // this is an address on campus, so we'll set the address to the campus address
            address.AddressLine2 = $"{address.AddressLine1}, {address.AddressLine2}";
            address.AddressLine1 = "7500 West Camp Wisdom Road";
            address.PostalCode = "75236";
        }
    }

    if (address.AddressLine1 != null && address.AddressLine1.StartsWith("Cowan") &&  address.City == "Duncanville")
    {
        address.AddressLine2 = address.AddressLine1;
        address.AddressLine1 = "7500 West Camp Wisdom Road";
        address.City = "Dallas";
        address.PostalCode = "75236";
    }

    if (address != null && address.AddressLine1 != null)
    {
        address.AddressLine1 = address.AddressLine1.Replace("Gust House", "Guest House");
        address.AddressLine1 = address.AddressLine1.Replace("Guest Houe", "Guest House");
        address.AddressLine1 = address.AddressLine1.Replace("Guesthouse", "Guest House");
    }

    // mobile home or guest house address on campus - AddressLine1
    if (address.AddressLine1 != null && (address.AddressLine1.StartsWith("MH") || address.AddressLine1.StartsWith("Mobile Home") || address.AddressLine1.StartsWith("GH") || 
        address.AddressLine1 == "B-3B" || address.AddressLine1 == "D13" || address.AddressLine1.StartsWith("Guest House") ||
        address.AddressLine1.StartsWith("RV") || address.AddressLine1.StartsWith("R V ") || address.AddressLine1.StartsWith("MH") ||
        address.AddressLine1.StartsWith("Cowan") || address.AddressLine1.StartsWith("12 Plex")))
    {
        // added check for fat-fingered postal code 75136 to cover three addresses internal to ILC (2 RV, 1 Guest House)
        if ((string.IsNullOrEmpty(address.PostalCode) || address.PostalCode == "75236" || address.PostalCode == "75136") && (string.IsNullOrEmpty(address.City) || address.City == "Dallas") &&
            (string.IsNullOrEmpty(address.StateProvince) || address.StateProvince == "TX"))
        {
            // this is an address on campus, so we'll set the address to the campus address
            address.AddressLine2 = address.AddressLine1;
            address.AddressLine1 = "7500 West Camp Wisdom Road";
            address.PostalCode = "75236";
        }
    }

    if ((address.PostalCode ?? "").Contains("(??)"))
    {
        address.PostalCode = address.PostalCode.Replace("(??)", "").Trim();
    }

    // we have already trimmed spaces, so we can check for length and if it contains an embedded space
    if ((address.PostalCode ?? "").Contains(" "))
    {
        // check for canadian / UK postal code
        if (address.PostalCode.Length == 7 && address.PostalCode[3] == ' ')
        {
            if (address.PostalCode.EndsWith(" UK"))
            {
                address.Country = "United Kingdom";
            }
            else
                address.Country = "Canada";
            address.PostalCode = address.PostalCode.Substring(0, 3) + address.PostalCode.Substring(4, 3);
        }
    }
    // Cumbria	LA15 8QA	United Kingdom
    if (address.PostalCode == "LA15 8QA")
    {
        address.City = "Cumbria";
        address.Country = "United Kingdom";
    }

    if (address.AddressLine1 == "7233 Benchar" && address.StateProvince == "BC")
    {
        address.PostalCode = "V2K5A2";
        address.Country = "Canada";
        address.AddressLine1 = "7233 Bench Dr";
    }
    if (address.AddressLine1 == "1031 Huntington Dr" && address.City == "Duncanville")
    {
        address.StateProvince = "TX";
        address.PostalCode = "75137";
    }
    if (address.AddressLine1 == "4798 Rabb Pl")
    {
        address.City = "Greensboro";
        address.StateProvince = "NC";
        address.PostalCode = "27410";
        address.Country = "United States of America";
    }
    if (address.PostalCode == "T4ROB4")
    {
        address.PostalCode = "T4R0B4";
    }
    if (address.StateProvince == "BC, Canada")
    {
        address.StateProvince = "BC";
        address.Country = "Canada";
    }
    if (address.City == "Altona, MB  ROG OB7")
    {
        address.City = "Altona";
        address.StateProvince = "MB";
        address.PostalCode = "R0G0B7";
        address.Country = "Canada";
    }
    if (address.City == "Hamilton, Ontario")
    {
        address.City = "Hamilton";
        address.StateProvince = "ON";
        address.Country = "Canada";
    }
    if (address.PostalCode == "TOB-OBO" && address.AddressLine1 == "PO Box 114")
    {
        address.StateProvince = "AB";
        address.PostalCode = "T0B0B0";
        address.Country = "Canada";
    }
    if (address.StateProvince == "Bangkok 10400, THAILAND")
    {
        address.StateProvince = null;
        address.City = "Bangkok 10400";
        address.Country = "Thailand";
    }
    if ((new string [] {"MB CA", "MB Canada", "MB, Canada"}).Contains(address.StateProvince))
    {
        if (address.PostalCode == "ROA0C0")
            address.PostalCode = "R0A0C0";
        if (address.PostalCode == "R0K6OL")
            address.PostalCode = "R0K0E0";
        if (address.City.Contains("(??)"))
            address.City = address.City.Replace("(??)", "").Trim();

        address.StateProvince = "MB";
        address.Country = "Canada";
    }
    if (address.StateProvince == "Manitoba")
    {
        address.StateProvince = "MB";
    }
    if (address.StateProvince == "ON, Canada")
    {
        address.StateProvince = "ON";
        address.Country = "Canada";
    }
    if (address.StateProvince == "PEI, Canada")
    {
        address.StateProvince = "PE";
        address.Country = "Canada";
    }
    if (address.StateProvince == "SK S4N4P6")
    {
        address.StateProvince = "SK";
        address.PostalCode = "S4N4P6";
        address.Country = "Canada";
    }
    if (address.City == "Dallas" && address.StateProvince == "TS" && address.PostalCode == "75236")
    {
        address.StateProvince = "TX";
    }
    if (address.StateProvince == "VIC, Australia")
    {
        address.StateProvince = "VIC";
        address.Country = "Australia";
    }
    // Saskatchewan S0G 4A0
    if (address.StateProvince == "Saskatchewan")
    {
        address.StateProvince = "SK";
        address.Country = "Canada";
        address.PostalCode = "S0G4A0";
    }
    //Scotland EH49 7TD
    if (address.StateProvince == "Scotland EH49 7TD")
    {
        address.StateProvince = null;
        address.PostalCode = "EH49 7TD";
        address.Country = "Scotland";
    }
    if (address.AddressLine1 == "28 Varcrest Pl NW" && address.City == "Calgary" && address.PostalCode == "T3A0B9")
    {
        address.StateProvince = "AB";
        address.Country = "Canada";
    }
    if (address.StateProvince == "AB/Canada" && address.PostalCode == "TIA8V4")
    {
        address.StateProvince = "AB";
        address.PostalCode = "T1A8V4";
    }
    if (address.StateProvince == "75104" || address.StateProvince == "75236")
    {
        address.PostalCode = address.StateProvince;
        address.StateProvince = "TX";
    }
    if (address.StateProvince == "Ontario LOR IWO")
    {
        address.Country = "Canada";
        address.City = "Mount Hope";
        address.StateProvince = "ON";
        address.PostalCode = "L0R 1W0";
    }
    if (address.StateProvince == "ON CANADA")
    {
        address.StateProvince = "ON";
    }
    if (address.StateProvince == "TX75211")
    {
        address.StateProvince = "TX";
        address.PostalCode = "75211";
    }
    if (address.StateProvince == "France") // one row in the source data had France as the state and a french street and postal code
    {
        address.Country = "France";
        address.StateProvince = null;
    }
    if (address.StateProvince == "PNG")
    {
        address.City = "Ukarumpa";
        address.StateProvince = "EHP 444";
        address.Country = "Papua New Guinea";
        address.PostalCode = null;
    }
    if (address.StateProvince == "Chad, Africa")
    {
        address.StateProvince = null;
        address.PostalCode = null;
        address.Country = "Chad, Africa";
    }
    if (address.StateProvince == "Victoria")
    {
        address.StateProvince = "VIC";
        address.Country = "Australia";
    }
    if ((address.StateProvince ?? "").Contains(", Mexico")) // one row Puebla, Mexico - trying to make it generic in case another shows up when I update
    {
        address.Country = "Mexico";
        address.StateProvince = address?.StateProvince?.Replace(", Mexico", "");
    }

    if (address != null && address.AddressLine1 != null &&
        (address.AddressLine1.ToLower() == "cancelled" || address.AddressLine1.ToLower().StartsWith("not coming")))
    {
        Console.WriteLine($"Address with DDDId {address.DDDId} has AddressLine1 of 'Cancelled' or 'not coming'. Skipped.");
        return null;
    }

    // create regex to check if postalcode is numeric and either 5 or 9 digits, or 5 digits with a dash and then 4 digits
    Regex regexZipCode = new Regex(@"^\d{5}(?:[-\s]\d{4})?$");
    if (address.PostalCode != null && regexZipCode.IsMatch(address.PostalCode) && address.Country != "France" && address.Country != "Mexico")
    {
        // some zip codes were fat-fingered in the source data, so we'll fix them here
        if (address.PostalCode == "76236" || address.PostalCode == "75136" || address.PostalCode == "74236") // these rows' zips don't exist and in the data looked like ILC or nearby in dallas 75236
            address.PostalCode = "75236";
        if (address.PostalCode == "76056") // crowley
            address.PostalCode = "76036";
        if (address.PostalCode == "76851") // grapevine
            address.PostalCode = "76051";
        if (address.PostalCode == "79137" || address.PostalCode == "78137") // address row that had duncanville listed as the city - so 75137
            address.PostalCode = "75137";
        if (address.PostalCode == "75674")
            address.PostalCode = "76574";
        // 704 Andy St, Springdale, AR 72762
        if (address.PostalCode == "76762")
        { 
            address.PostalCode = "72762";
            address.City = "Springdale";
            address.StateProvince = "AR";
        }

        // okay - zip, city and address were all wrong - found the address locally by looking up the person's org
        if (address.AddressLine1 == "2525 Sweebriar") // one in person directory address and one in address addressline1
        {
            address.AddressLine1 = "2525 Sweetbriar Dr";
            address.City = "Dallas";
            address.StateProvince = "TX";
            address.PostalCode = "75228";
        }

        // we have a zip code, so we can look up the state and city.
        // Some of the US addresses had city data in the state insterad of the state - Look them up if they are > 2 chars
        if (address.StateProvince == null || address.StateProvince.Length > 2)
            address.StateProvince = USZipCodeStateCityLookup.GetStateByZip(address.PostalCode.Substring(0,5));
        if (address.City == null)
            address.City = USZipCodeStateCityLookup.GetCityByZip(address.PostalCode.Substring(0, 5));

        address.Country = "United States of America";
    }

    // 6924 Rolling Creek Ln, Dallas, TX 75236, United States
    if (address.AddressLine1 == "6924 Rolling Crk Ln")
    {
        address.AddressLine1 = "6924 Rolling Creek Ln";
        address.City = "Dallas";
        address.StateProvince = "TX";
        address.PostalCode = "75236";
        address.Country = "United States of America";
    }

    // "1st Presby -D'ville" - 543 E Freeman St, Duncanville, TX 75116
    if (address.AddressLine1 == "1st Presby -D'ville")
    {
        address.AddressLine1 = "1st Presbyterian Church";
        address.AddressLine2 = "543 E Freeman St";
        address.City = "Duncanville";
        address.StateProvince = "TX";
        address.PostalCode = "75116";
        address.Country = "United States of America";
    }


    // 442 Morning Dove Dr, Duncanville, TX 75137, United States
    if (address.AddressLine1 == "442 Morning Dove")
    {
        address.AddressLine1 = "442 Morning Dove Dr";
        address.City = "Duncanville";
        address.StateProvince = "TX";
        address.PostalCode = "75137";
        address.Country = "United States of America";
    }

    //if (address2.City == null)
    //    Debug.Assert(false, "City is null");

    //if (address2.State == null)
    //    Debug.Assert(false, "State is null");

    //if (address2.AddressLine1 == null)
    //    Debug.Assert(false, "AddressLine1 is null");

    // Since we are low on time, we'll mark any address that doesn't have a city, state and address as needing to be fixed
    if (address.Country == "United States of America")
    {
        if (string.IsNullOrEmpty(address.City))
            address.City = "Migration:TOFIX";
        if (string.IsNullOrEmpty(address.StateProvince))
            address.StateProvince = "Migration: TOFIX";
        if (string.IsNullOrEmpty(address.AddressLine1))
            address.AddressLine1 = "Migration: TOFIX";
        if (string.IsNullOrEmpty(address.PostalCode))
            address.PostalCode = "Migration: TOFIX";
    }

    // ContactPerson is simply a string name of the person managing the property and is not included in the directory people table
    address.ContactPersonName = contactPerson;
    address.ContactPersonPhone = contactPhone;
    address.IncludeInDirectory = directoryInclude; // use the person's directory include flag for initial value
    address.CreateDateTime = DateTimeOffset.Now;
    address.ModifiedDateTime = DateTimeOffset.Now;
    address.ModifiedByUserName = modifiedByUserName;

    if (address.PostalCode == "")
        address.PostalCode = null;
    address.IsVerified = false;

    // Address1401 Commerce Street, Dallas, TX 75201
    if (address.AddressLine1 == "Magnolia Hotel" && (address.StateProvince == "TX" || address.StateProvince == null) && 
        address.City == null && address.PostalCode == null)
    {
        address.City = "Dallas";
        address.PostalCode = "75201";
    }

    // 3475 Riverstone Ct, Fort Worth, TX 76116
    if (address.AddressLine1 == "3475 Riverstone Ct #1614" && address.StateProvince == null && address.City == null && address.PostalCode == null)
    {
        address.City = "Fort Worth";
        address.StateProvince = "TX";
        address.PostalCode = "76116";
    }

    // 1000 Texan Trail, Grapevine, TX 76051, United States
    if (address.AddressLine1 == "Capin Crouse LLP")
    {
        address.AddressLine1 = "1000 Texan Trail";
        address.City = "Grapevine";
        address.StateProvince = "TX";
        address.PostalCode = "76051";
        address.Country = "United States of America";
    }

    // 7575 S Westmoreland Rd #2024, Dallas, TX 75237
    if (address.AddressLine1 == "7575 S Westmoreland Rd #2024" && address.StateProvince == null && address.City == null && address.PostalCode == null)
    {
        address.City = "Dallas";
        address.StateProvince = "TX";
        address.PostalCode = "75237";
    }

    // Hilton Garden Inn, 800 North Main Street, Duncanville, TX 75116
    if (address.AddressLine1 == "Hilton Garden Inn" && address.StateProvince == null && address.City == null && address.PostalCode == null)
    {
        address.City = "Duncanville";
        address.StateProvince = "TX";
        address.PostalCode = "75116";
    }

    // 6653 Nyman Dr, Dallas, TX 75236
    if (address.AddressLine1 == "6653 Nyman Dr" && address.StateProvince == null && address.City == null && address.PostalCode == null)
    {
        address.City = "Dallas";
        address.StateProvince = "TX";
        address.PostalCode = "75236";
    }

    // Motel 6, 202 Jellison Blvd, Duncanville, TX 75116
    if (address.AddressLine1 == "Motel 6" && address.StateProvince == null && address.City == null && address.PostalCode == null)
    {
        address.AddressLine2 = "202 Jellison Blvd";
        address.City = "Duncanville";
        address.StateProvince = "TX";
        address.PostalCode = "75116";
    }

    if (address.AddressLine1 == "w/ non-SIL friends" && address.StateProvince == "TX")        
    {
        Console.WriteLine($"{Environment.NewLine}Incomplete Address given for DDDId = {address.DDDId}, Address Note: {address.AddressLine1}, skipped");
        return null;
    }

    // 7344 Zuni Dr. Apt. 106, Dallas, TX 75237
    if (address.AddressLine1 == "7344 Zuni Dr. Apt. 106" && address.StateProvince == null && address.City == null && address.PostalCode == null)
    {
        address.AddressLine1 = "7344 Zuni St";
        address.AddressLine2 = "Apt 106";
        address.City = "Dallas";
        address.StateProvince = "TX";
        address.PostalCode = "75237";
    }

    // c/0 Carolynn Parker @ Cowan
    if (address.AddressLine1 == "c/0 Carolynn Parker @ Cowan")
        address.AddressLine1 = "c/o Carolynn Parker @ Cowan";

    // co0 Brenda Boerger
    if (address.AddressLine1 == "co0 Brenda Boerger")
        address.AddressLine1 = "c/o Brenda Boerger";

    if (address.City == null && address.StateProvince == null && address.PostalCode == null &&
        (address.AddressLine1 != null && (address.AddressLine1.ToLower().Contains("remote from") || 
            address.AddressLine1.ToLower().Contains("rem from") || address.AddressLine1.ToLower().StartsWith("apt ") ||
            address.AddressLine1.StartsWith("c/o") || address.AddressLine1 == "Now in Alabama" || 
            address.AddressLine1 == "Comes & Goes" || address.AddressLine1 == "3939 FM 2194" ||
            address.AddressLine1.ToLower() == "off center" || address.AddressLine1.ToLower() == "off campus" ||
            address.AddressLine1 == "Remote" || address.AddressLine1 == "Moved out of state" ||
            address.AddressLine1 == "Ck Welcome Desk" || address.AddressLine1 == "Returned to INDIA" || 
            address.AddressLine1.StartsWith("w/") || address.AddressLine1 == "UIDS office" ||
            address.AddressLine1 == "with Lorna" || address.AddressLine1 == "teaches DIU remotely" ||
            address.AddressLine1 == "12" || address.AddressLine1.ToLower() == "hotel" || 
            address.AddressLine1 == "2013 Mossberg St" || address.AddressLine1 == "shiloh_peterson@sil.org" ||
            address.AddressLine1 == "kimlovesalbania@hotmail.com" || address.AddressLine1 == "." ||
            address.AddressLine1.ToLower() == "not indicated" || address.AddressLine1.ToLower() == "not given" ||
            address.AddressLine1.ToLower().StartsWith("unknown"))))
     {
        Console.WriteLine($"{Environment.NewLine}Incomplete Address given for DDDId = {address.DDDId}, Address Note: {address.AddressLine1}, skipped");
        return null;
    }

    if (address != null && address.AddressLine1 == null && address.AddressLine2 == null && address.PostalCode == null &&
        !string.IsNullOrEmpty(address.City))
    {
        Console.WriteLine($"{Environment.NewLine}Incomplete - Only City given for DDDId = {address.DDDId}, Address Note: {address.AddressLine1}, skipped");
        return null;
    }

    if (address != null && address.AddressLine1 == null && address.AddressLine2 == null && address.PostalCode == null &&
        address.City == null && !string.IsNullOrEmpty(address.StateProvince))
    {
        Console.WriteLine($"{Environment.NewLine}Incomplete - Only State given for DDDId = {address.DDDId}, Address Note: {address.StateProvince}, skipped");
        return null;
    }

    if (address.AddressLine1 == null && address.City == null && address.StateProvince == null && address.PostalCode == null)
        return null;

    if (address.City == null)
        Console.WriteLine();

    var personHousehold = allPersonHouseholdRows.FirstOrDefault(x => x.PersonId == id);
    var householdAddresses = allHouseholdAddressRows.Where(x => x.HouseholdId == personHousehold?.HouseholdId);

    foreach (var householdAddress in householdAddresses)
    {
        var existingAddress = allAddressRows.FirstOrDefault(x => x.AddressId == householdAddress.AddressId);

        if (allAddressRows.Any(x => x.AddressLine1 == address.AddressLine1 && x.City == address.City && x.StateProvince == address.StateProvince))
            return null; // we already have this address for this person's household
    }

    return address;
}




