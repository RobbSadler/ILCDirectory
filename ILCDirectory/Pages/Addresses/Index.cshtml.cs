namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAddressRepository _addressRepo;
        public IList<Address> Addresses { get;set; }

        public IndexModel(IAddressRepository addressRepo)
        {
            _addressRepo = addressRepo;
        }

        public async Task OnGetAsync()
        {
            Addresses = (IList<Address>)await _addressRepo.GetAllAsync();
        }
    }
}
