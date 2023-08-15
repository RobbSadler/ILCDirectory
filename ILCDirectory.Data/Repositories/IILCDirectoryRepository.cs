using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories
{
    public interface IILCDirectoryRepository
    {
        Task<Person> GetPersonAsync(int? id);
        Task<IList<Person>> GetAllPersonsAsync();
        Task<Person> FindPersonAsync(string searchTerm);

        Task<Address> GetAddressAsync(int? id);
        Task<IList<Address>> GetAllAddressesAsync();
        Task<IList<Address>> FindAddressesAsync(string searchTerm);
    }
}
