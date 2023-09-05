namespace ILCDirectory.Pages.People
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;
        private readonly IConfiguration _cfg;

        public EditModel(IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _repo = repo;
            _cfg = cfg;
        }

        [BindProperty]
        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _repo.GetPersonAsync(_cfg, id);

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

            //await _repo.UpdatePersonAsync(_cfg,Person);

            return RedirectToPage("./Index");
        }
    }
}
