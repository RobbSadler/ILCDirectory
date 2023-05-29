namespace ILCDirectory.Views.People
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly IPersonRepository _personRepo;

        public DeleteModel(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _personRepo.GetAsync(id);

            if (Person != null)
            {
                await _personRepo.DeleteAsync(Person);
            }

            return RedirectToPage("./Index");
        }
    }
}
