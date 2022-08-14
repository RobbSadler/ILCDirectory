using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ILCDirectory.Data
{
    public class OfficeDetailsRepository : GenericRepository<OfficeDetails>, IOfficeDetailsRepository
    {
        public OfficeDetailsRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "OfficeDetails")
        {
        }
    }
}
