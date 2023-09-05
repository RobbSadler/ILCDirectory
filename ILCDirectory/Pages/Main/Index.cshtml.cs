using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.Main
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;
        private readonly IConfiguration _cfg;

        public IndexModel(IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _repo = repo;
            _cfg = cfg;
        }

        public IList<Person> Persons { get; set; }
        public Person SelectedPerson { get; set; }
        public Address SelectedAddress { get; set; }

        public Family Family { get; set; }

        public async Task OnGetAsync()
        {
            Persons = await _repo.GetAllPersonsAsync(_cfg);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task OnPostAsync()
        {
            RedirectToPage("/Main");
        }
    }
}
