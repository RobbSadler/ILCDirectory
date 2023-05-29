namespace ILCDirectory.Data.Repositories
{
    public class OtherMailRepository : GenericRepository<OtherMail>, IOtherMailRepository
    {
        public OtherMailRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "OtherMail")
        {

        }
    }
}
