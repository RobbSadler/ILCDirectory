namespace ILCDirectory.Data.Repositories
{
    public class DeliveryCodeLocationRepository : GenericRepository<DeliveryCodeLocation>, IDeliveryCodeLocationRepository
    {
        public DeliveryCodeLocationRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "DeliveryCodeDescription")
        {

        }
    }
}
