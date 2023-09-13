using ILCDirectory.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IILCDirectoryRepository _repo;
        private readonly IConfiguration _cfg;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _cfg = cfg;
        }

        public IList<Person> Persons { get; set; }

        public async Task OnGet()
        {
            Persons = await _repo.GetAllPersonsAsync(_cfg);
        }
    }
}