namespace ILCDirectory.Data.Repositories
{
    public interface ITokenizeAndSearchRepository
    {
        Task<(IList<Person>, IList<HouseholdAddress>)> SearchForPersonOrAddress(IConfiguration config, string searchString, bool searchPartialWords,
            bool includeChildren, bool localOnly);
        Task PopulateSearchTokenTables(IConfiguration config);
    }
}