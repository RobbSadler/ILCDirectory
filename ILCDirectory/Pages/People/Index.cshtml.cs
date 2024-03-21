namespace ILCDirectory.Pages.People
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;

        public IndexModel(IILCDirectoryRepository repo)
        {
            _repo = repo;
        }

        public IList<Person> Persons { get; set; }
        public Household Family { get; set; }

        public async Task OnGetAsync()
        {
            Persons = await _repo.GetAllRowsAsync<Person>("Person");
        }
    }
}
