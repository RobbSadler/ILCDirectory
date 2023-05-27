namespace ILCDirectory.Data.Repositories
{
    public class DeliveryCodeRepository : GenericRepository<DeliveryCodeDescription>, IDeliveryCodeRepository
    {
        public DeliveryCodeRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "DeliveryCode")
        {

        }
    }
}
