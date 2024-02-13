using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories
{
    public interface IILCDirectoryRepository
    {
        Task<IList<T>> GetAllRowsAsync<T>(string tableName) where T : new();
        Task<T> GetRowByIdAsync<T>(int id, string tableName) where T : new();
        Task<IList<T>> GetRowsByIdsAsync<T>(IList<int> ids, string tableName) where T : new();
        Task<T> InsertRowAsync<T>(T rowData, string tableName) where T : new();
        Task<T> UpdateRowAsync<T>(T rowData, List<string> keyNames, string tableName) where T : new();
        Task DeleteRowAsync<T>(int id, string tableName) where T : new();
        Task DeleteRowsAsync<T>(IList<int> ids, string tableName) where T : new();
        Task<Building> InsertBuildingAsync(Building building, bool identityInsert = false);
        Task<Classification> InsertClassificationAsync(Classification classification, bool identityInsert = false);
        Task<Person> InsertPersonAsync(Person person, bool identityInsert = false);
        Task<DeliveryCodeLocation> InsertDeliveryCodeLocationAsync(DeliveryCodeLocation deliveryCodeLocation, bool identityInsert = false);
        Task<List<ParentChild>> GetParentChildFromParentIdAsync(int parentId);
        Task<List<ParentChild>> GetParentChildFromChildIdAsync(int childId);
        Task<PersonFamilyAddressDetails> GetPersonFamilyAddressDetailsAsync(int personId, int? spousePersonId);
        Task<PersonFamilyDetails> GetPersonFamilyDetailsAsync(int personId, int? spousePersonId);
        Task<PersonAddressDetails> GetPersonAddressDetailsAsync(int personId);
        Task<IList<Email>> GetPersonEmailsAsync(int personId);
        Task<List<Person>> GetParentPersonsFromChildAsync(int childId);
        Task<List<PhoneNumber>> GetPersonPhonesAsync(int personId);
        Task<List<PersonHousehold>> GetPersonHouseholdsAsync(int personId);
        Task<List<Person>> GetChildPersonsForParentAsync(int parentId);
        Task<InternalAddress> GetInternalAddressForPersonAsync(int personId);
        Task<Household> GetHouseholdForPersonAsync(int personId);
        Task<List<int>> GetPersonIdsForHouseholdAsync(int householdId);
        Task<List<HouseholdAddress>> GetHouseholdAddressesForPersonAsync(int personId);
        Task<List<Person>> GetChildrenForPersonAsync(int personId);
        Task<List<Person>> GetParentsForPersonAsync(int personId);
    }
}
