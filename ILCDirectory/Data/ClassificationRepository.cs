namespace ILCDirectory.Data
{
    public class ClassificationRepository : GenericRepository<Classification>, IClassificationRepository
    {
        public ClassificationRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Classification")
        {

        }
    }
}
