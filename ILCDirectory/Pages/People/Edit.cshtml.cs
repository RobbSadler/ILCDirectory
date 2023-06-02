namespace ILCDirectory.Pages.People
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IPersonRepository _personRepo;

        public EditModel(IPersonRepository personRepo)
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _personRepo.UpdateAsync(Person);

            return RedirectToPage("./Index");
        }
    }
}
