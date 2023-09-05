using ILCDirectory.Data.Repositories;

namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IILCDirectoryRepository _ilcDirectoryRepository;
        public IList<Address> Addresses { get;set; }

        public IndexModel(IILCDirectoryRepository ilcDirectoryRepository)
        {
            _ilcDirectoryRepository = ilcDirectoryRepository;
        }

        public async Task OnGetAsync()
        {
            Addresses = await _ilcDirectoryRepository.GetAllAddressesAsync();
        }
    }
}
