// See https://aka.ms/new-console-template for more information
using ILCDirectory.Data;
using ILCDirectory.Data.Models;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Reflection;

using (OleDbConnection connect = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\robb\\Downloads\\DDD data-20220302T224527Z-001\\DDD data\\DDD-tables.accdb;Persist Security Info=False;"))
{
    var configurationBuilder = new ConfigurationBuilder();
    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
    configurationBuilder.AddUserSecrets(typeof(Program).GetTypeInfo().Assembly, optional: false);
    IConfigurationRoot config = configurationBuilder.Build();

    // lookup dictionaries
    var titleDictionary = new Dictionary<int, string>();
    var classificationDictionary = new Dictionary<int, Classification>();

    connect.Open();

    // Title
    OleDbCommand cmdTitle = new OleDbCommand("select * from tblTitle", connect);
    OleDbDataAdapter daTitle = new OleDbDataAdapter(cmdTitle);
    DataSet dsetTitle = new DataSet();
    daTitle.Fill(dsetTitle);
    foreach (DataRow titleRow in dsetTitle.Tables[0].Rows)
    {
        titleDictionary.Add((int)titleRow["TitleId"], (string)titleRow["TitleName"]);
    }

    // Classification
    OleDbCommand cmdClassification = new OleDbCommand("select * from tblClassification", connect);
    OleDbDataAdapter daClassification = new OleDbDataAdapter(cmdClassification);
    DataSet dsetClassification = new DataSet();
    daClassification.Fill(dsetClassification);
    foreach (DataRow classificationRow in dsetClassification.Tables[0].Rows)
    {
        classificationDictionary.Add(
            (int)classificationRow["ClassificationId"], 
            new Classification() {
                ClassificationId = (int)classificationRow["ClassificationID"],
                ClassificationCode = (string)classificationRow["StatusCode"],
                Description = (string)classificationRow["StatusDescription"]
            });
    }

    // Person
    OleDbCommand personCmd = new OleDbCommand("select TOP 1 * from Person", connect);
    OleDbDataAdapter daPerson = new OleDbDataAdapter(personCmd);
    DataSet dsetPerson = new DataSet();
    daPerson.Fill(dsetPerson);

    var personRepo = new PersonRepository(config);
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
    }

    // select TOP 1000 * from person p inner join Addresses a ON a.ID = p.ID ORDER BY p.ID DESC
    // 


    connect.Close();
}