using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories
{
    public interface IILCDirectoryRepository
    {
        Task<Person> GetPersonAsync(IConfiguration config, int? id);
        Task<IList<Person>> GetAllPersonsAsync(IConfiguration config);
        //Task<Person> FindPersonAsync(IConfiguration config, string searchTerm);

        Task<Address> GetAddressAsync(IConfiguration config, int? id);
        Task<IList<Address>> GetAllAddressesAsync(IConfiguration config);
        //Task<IList<Address>> FindAddressesAsync(IConfiguration config, string searchTerm);
    }
}
