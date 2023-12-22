namespace ILCDirectory.Data.Repositories
{
    public interface ITokenizeAndSearchRepository
    {
        Task<(IList<Person>, IList<Address>)> SearchForPersonOrAddress(IConfiguration config, string searchString);
        Task PopulateSearchTokenTables(IConfiguration config);
    }
}