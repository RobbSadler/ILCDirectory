using ILCDirectory.Data.Repositories;

namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;
        public IList<Address> Addresses { get;set; }

        public IndexModel(IILCDirectoryRepository repo)
        {
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            Addresses = await _repo.GetAllRowsAsync<Address>("Address");
        }
    }
}
