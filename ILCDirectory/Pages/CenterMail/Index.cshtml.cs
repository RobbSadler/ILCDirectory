using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.CenterMail
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
            // load cookie value for last person being edited
            //if (Request.Cookies.ContainsKey("lastPersonId"))
            //{
            //    var lastPersonId = Request.Cookies["lastPersonId"];

            //}
            Persons = await _repo.GetAllPersonsAsync(_cfg);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task OnPostAsync()
        //{
        //    RedirectToPage("/Main");
        //}
    }
}
