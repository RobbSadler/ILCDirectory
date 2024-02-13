namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;

        public DetailsModel(IILCDirectoryRepository repo)
        {
            _repo = repo;
        }

        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Address = await _repo.GetRowByIdAsync<Address>((int)id, "Address");

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
