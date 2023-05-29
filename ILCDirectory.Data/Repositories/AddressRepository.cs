namespace ILCDirectory.Data.Repositories
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Address")
        {
        }
    }
}
