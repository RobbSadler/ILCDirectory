namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;
        private readonly IConfiguration _cfg;
        public IList<Address> Addresses { get;set; }

        public IndexModel(IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _repo = repo;
            _cfg = cfg;
        }

        public async Task OnGetAsync()
        {
            Addresses = await _repo.GetAllAddressesAsync(_cfg);
        }
    }
}
