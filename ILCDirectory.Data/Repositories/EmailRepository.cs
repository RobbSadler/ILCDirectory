namespace ILCDirectory.Data.Repositories
{
    public class EmailRepository : GenericRepository<Email>, IEmailRepository
    {
        public EmailRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Email")
        {

        }
    }
}
