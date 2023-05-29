namespace ILCDirectory.Data.Repositories
{
    public class OfficeDetailsRepository : GenericRepository<OfficeDetails>, IOfficeDetailsRepository
    {
        public OfficeDetailsRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "OfficeDetails")
        {
        }
    }
}
