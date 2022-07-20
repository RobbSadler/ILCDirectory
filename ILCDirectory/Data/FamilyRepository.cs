namespace ILCDirectory.Data
{
    public class FamilyRepository : GenericRepository<Family>, IFamilyRepository
    {
        public FamilyRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Family")
        {

        }
    }
}
