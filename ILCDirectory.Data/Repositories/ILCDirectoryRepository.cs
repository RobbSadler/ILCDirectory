using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories
{
    public class ILCDirectoryRepository : IILCDirectoryRepository
    {
        public async Task<Person> GetPersonAsync(int? id)
        {

        }
        public async Task<IList<Person>> GetAllPersonsAsync()
        {

        }
        public async Task<Person> FindPersonAsync(string searchTerm)
        {

        }

        public async Task<Address> GetAddressAsync(int? id)
        {

        }
        public async Task<IList<Address>> GetAllAddressesAsync()
        {

        }
        public async Task<IList<Address>> FindAddressesAsync(string searchTerm)
        {

        }
    }
}
