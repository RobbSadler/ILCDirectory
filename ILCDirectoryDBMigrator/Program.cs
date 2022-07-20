// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Data.OleDb;

Console.WriteLine("Hello, World!");

using (OleDbConnection connect = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\Users\\robb\\Downloads\\DDD data-20220302T224527Z-001\\DDD data\\DDD-tables.accdb;Persist Security Info=False;"))
{ 
    //param0.Value = employeeID.Text;
    //command.Parameters.Add(param0);

    connect.Open();

    // Person
    OleDbCommand command = new OleDbCommand("select * from Person", connect);
    OleDbDataAdapter da = new OleDbDataAdapter(command);
    DataSet dset = new DataSet();
    da.Fill(dset);
    dset.ToString();


    connect.Close();
}