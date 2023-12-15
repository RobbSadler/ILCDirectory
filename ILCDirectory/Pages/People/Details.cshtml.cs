namespace ILCDirectory.Pages.People
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

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _repo.GetRowByIdAsync<Person>(_cfg, (int)id, "Person");

            if (Person == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
