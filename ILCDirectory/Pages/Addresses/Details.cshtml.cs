namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IILCDirectoryRepository _ilcDirectoryRepository;
        private readonly IConfiguration _configuration;

        public DetailsModel(IConfiguration configuration, IILCDirectoryRepository ilcDirectoryRepository)
        {
            _ilcDirectoryRepository = ilcDirectoryRepository;
            _configuration = configuration;
        }

        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var connectionString = _configuration["ILCDirectory:ConnectionString"];
            using (var connection = new SqlConnection(connectionString))
            {

            }
            Address = await _addressRepo.GetAsync(id);

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
