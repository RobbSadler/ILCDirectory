namespace ILCDirectory.Data.Repositories
{
    public class WorkgroupRepository : GenericRepository<Workgroup>, IWorkgroupRepository
    {
        public WorkgroupRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Workgroup")
        {

        }
    }
}
