using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.Main
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly IILCDirectoryRepository _ilcDirectoryRepository;

        public IndexModel(IILCDirectoryRepository ilcDirectoryRepository)
        {
            _ilcDirectoryRepository = ilcDirectoryRepository;
        }

        public IList<Person> Persons { get; set; }
        public Family Family { get; set; }

        public async Task OnGetAsync()
        {
            Persons = (IList<Person>)(await _ilcDirectoryRepository.GetAllPersonsAsync());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task OnPostAsync()
        {
            RedirectToPage("/Main");
        }
    }
}
