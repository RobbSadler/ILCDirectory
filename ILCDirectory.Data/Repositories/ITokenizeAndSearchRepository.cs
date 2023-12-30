namespace ILCDirectory.Data.Repositories
{
    public interface ITokenizeAndSearchRepository
    {
        Task<IList<Person>> SearchForPersonOrAddress(IConfiguration config, string searchString, bool searchPartialWords,
            bool includeChildren, bool localOnly);
        Task PopulateSearchTokenTables(IConfiguration config);
    }
}