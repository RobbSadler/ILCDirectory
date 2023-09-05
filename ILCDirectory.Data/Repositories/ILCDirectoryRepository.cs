using ILCDirectory.Data.Models;
using SqlocityNetCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories
{
    public class ILCDirectoryRepository : IILCDirectoryRepository
    {
        private readonly IConfiguration _configuration;
        public ILCDirectoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Person> GetPersonAsync(int? id)
        {
            var conn = Sqlocity.CreateDbConnection(_configuration.GetConnectionString("ILCDirectoryConnection"));
            var cmd = Sqlocity.GetDatabaseCommand(conn);
            var person = await cmd.SetCommandText("SELECT * FROM Person WHERE Id = @Id")
                .AddParameter("@Id", id)
                .ExecuteToObjectAsync<Person>();
            return person;
        }
        public async Task<IList<Person>> GetAllPersonsAsync()
        {
            var conn = Sqlocity.CreateDbConnection(_configuration.GetConnectionString("ILCDirectoryConnection"));
            var cmd = Sqlocity.GetDatabaseCommand(conn);
            var persons = await cmd.SetCommandText("SELECT * FROM Person")
                .ExecuteToListAsync<Person>();
            return persons;
        }
        public async Task UpdatePerson(Person person)
        {
            var sql = @"UPDATE Person SET 
                PersonId = @PersonId,
                ChildOfFamilyId = @ChildOfFamilyId,
                ClassificationCode = @ClassificationCode,
                Comment = @Comment,
                DateOfBirth = @DateOfBirth,
                DeleteFlag = @DeleteFlag,
                FieldOfService = @FieldOfService,
                FirstName = @FirstName,
                Gender = @Gender,
                LanguagesSpoken = @LanguagesSpoken,
                LastName = @LastName,
                MaidenName = @MaidenName,
                MaritalStatus = @MaritalStatus,
                MiddleName = @MiddleName,
                NickName = @NickName,
                Notes = @Notes,
                ParentOfFamilyId = @ParentOfFamilyId,
                Position = @Position,
                SupervisorName = @SupervisorName,
                SupervisorNotes = @SupervisorNotes,
                Title = @Title,
                WoCode = @WoCode,
                WorkgroupCode = @WorkgroupCode
                WHERE PersonId = @PersonId";

            var conn = Sqlocity.CreateDbConnection(_configuration.GetConnectionString("ILCDirectoryConnection"));
            var cmd = Sqlocity.GetDatabaseCommand(conn);
            // use person object and sqlocity to update the database
            await cmd.SetCommandText(sql)
                .AddParameter("@PersonId", person.PersonId)
                .AddParameter("@ChildOfFamilyId", person.ChildOfFamilyId)
                .AddParameter("@ClassificationCode", person.ClassificationCode)
                .AddParameter("@Comment", person.Comment)
                .AddParameter("@DateOfBirth", person.DateOfBirth)
                .AddParameter("@DeleteFlag", person.DeleteFlag)
                .AddParameter("@FieldOfService", person.FieldOfService)
                .AddParameter("@FirstName", person.FirstName)
                .AddParameter("@Gender", person.Gender)
                .AddParameter("@LanguagesSpoken", person.LanguagesSpoken)
                .AddParameter("@LastName", person.LastName)
                .AddParameter("@MaidenName", person.MaidenName)
                .AddParameter("@MaritalStatus", person.MaritalStatus)
                .AddParameter("@MiddleName", person.MiddleName)
                .AddParameter("@NickName", person.NickName)
                .AddParameter("@Notes", person.Notes)
                .AddParameter("@ParentOfFamilyId", person.ParentOfFamilyId)
                .AddParameter("@Position", person.Position)
                .AddParameter("@SupervisorName", person.SupervisorName)
                .AddParameter("@SupervisorNotes", person.SupervisorNotes)
                .AddParameter("@Title", person.Title)
                .AddParameter("@WoCode", person.WoCode)
                .AddParameter("@WorkgroupCode", person.WorkgroupCode)
                .ExecuteNonQueryAsync();
        }

        //public async Task<Person> FindPersonAsync(string searchTerm)
        //{

        //}

        public async Task<Address> GetAddressAsync(int? id)
        {
            var conn = Sqlocity.CreateDbConnection(_configuration.GetConnectionString("ILCDirectoryConnection"));
            var cmd = Sqlocity.GetDatabaseCommand(conn);
            var address = await cmd.SetCommandText("SELECT * FROM Address WHERE Id = @Id")
                .AddParameter("@Id", id)
                .ExecuteToObjectAsync<Address>();
            return address;
        }
        public async Task<IList<Address>> GetAllAddressesAsync()
        {
            var conn = Sqlocity.CreateDbConnection(_configuration.GetConnectionString("ILCDirectoryConnection"));
            var cmd = Sqlocity.GetDatabaseCommand(conn);
            var addresses = await cmd.SetCommandText("SELECT * FROM Address")
                .ExecuteToListAsync<Address>();
            return addresses;
        }
        //public async Task<IList<Address>> FindAddressesAsync(string searchTerm)
        //{

        //}
    }
}
