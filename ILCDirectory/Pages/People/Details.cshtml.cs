namespace ILCDirectory.Pages.People
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;

        public DetailsModel(IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _repo = repo;
        }

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _repo.GetRowByIdAsync<Person>((int)id, "Person");

            if (Person == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
