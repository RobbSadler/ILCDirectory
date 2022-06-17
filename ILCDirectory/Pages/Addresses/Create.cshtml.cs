namespace ILCDirectory.Pages.Addresses
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Address Address { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(_configuration["ILCDirectory:ConnectionString"]))
            {
                connection.Open();
                var Persons = (await connection.QueryAsync("select * from People")).ToList();
            }

            return RedirectToPage("./Index");
        }
    }
}
