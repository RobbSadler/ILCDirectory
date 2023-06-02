namespace ILCDirectory.Pages.People
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IPersonRepository _personRepo;

        public DetailsModel(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
        }

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _personRepo.GetAsync(id);

            if (Person == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
