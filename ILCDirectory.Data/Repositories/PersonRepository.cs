namespace ILCDirectory.Data.Repositories
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Person")
        {
        }
    }
}
