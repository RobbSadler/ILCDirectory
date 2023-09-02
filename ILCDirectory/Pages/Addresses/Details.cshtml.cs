namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;
        private readonly IConfiguration _cfg;

        public DetailsModel(IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _repo = repo;
            _cfg = cfg;
        }

        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var connectionString = _cfg["ILCDirectory:ConnectionString"];
            using (var connection = new SqlConnection(connectionString))
            {

            }
            Address = await _repo.GetAddressAsync(_cfg, id);

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
