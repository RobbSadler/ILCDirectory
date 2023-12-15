using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data.Repositories
{
    public interface IILCDirectoryRepository
    {
        Task<IList<T>> GetAllRowsAsync<T>(IConfiguration config, string tableName) where T : new();
        Task<T> GetRowByIdAsync<T>(IConfiguration config, int id, string tableName) where T : new();
        
    }
}
