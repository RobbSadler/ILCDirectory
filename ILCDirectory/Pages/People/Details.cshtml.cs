namespace ILCDirectory.Pages.People
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly IILCDirectoryRepository _ilcDirectoryRepository;

        public DetailsModel(IILCDirectoryRepository ilcDirectoryRepository)
        {
            _ilcDirectoryRepository = ilcDirectoryRepository;
        }

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _ilcDirectoryRepository.GetAsync(id);

            if (Person == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
