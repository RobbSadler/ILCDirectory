namespace ILCDirectory.Data
{
    public class WoRepository : GenericRepository<Wo>, IWoRepository
    {
        public WoRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Wo")
        {

        }
    }
}
