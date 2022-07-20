namespace ILCDirectory.Data
{
    public class CityCodeRepository : GenericRepository<CityCode>, ICityCodeRepository
    {
        public CityCodeRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "CityCode")
        {

        }
    }
}
