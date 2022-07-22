namespace ILCDirectory.Data
{
    public class BuildingRepository : GenericRepository<Building>, IBuildingRepository
    {
        public BuildingRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Building")
        {

        }
    }
}
