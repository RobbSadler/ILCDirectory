using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.SetupConfiguration
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IILCDirectoryRepository _repo;

        public IndexModel(ILogger<IndexModel> logger, IILCDirectoryRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public IList<Person> Persons { get; set; }
        public Person SelectedPerson { get; set; }
        public Address SelectedAddress { get; set; }

        public Household Family { get; set; }

        public async Task OnGetAsync()
        {
            // load cookie value for last person being edited
            //if (Request.Cookies.ContainsKey("lastPersonId"))
            //{
            //    var lastPersonId = Request.Cookies["lastPersonId"];

            //}
            Persons = await _repo.GetAllRowsAsync<Person>("Person");
        }

        //[HttpPost]
        //[AllowAnonymous]
        //public async Task OnPostAsync()
        //{
        //    RedirectToPage("/Main");
        //}
    }
}
