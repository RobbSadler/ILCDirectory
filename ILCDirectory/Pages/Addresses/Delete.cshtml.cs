namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IAddressRepository _addressRepo;

        public DeleteModel(IAddressRepository addressRepo)
        {
            _addressRepo = addressRepo;
        }

        [BindProperty]
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _addressRepo.GetAsync(id);

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _addressRepo.GetAsync(id);

            if (Address != null)
            {
                await _addressRepo.DeleteAsync(Address.AddressId);
            }

            return RedirectToPage("./Index");
        }
    }
}
