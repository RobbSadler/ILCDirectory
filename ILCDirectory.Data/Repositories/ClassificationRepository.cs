namespace ILCDirectory.Data.Repositories
{
    public class ClassificationRepository : GenericRepository<Classification>, IClassificationRepository
    {
        public ClassificationRepository(IConfiguration configuration) : base(configuration[Constants.CONFIG_CONNECTION_STRING], "Classification")
        {

        }
    }
}
