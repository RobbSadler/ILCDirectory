using Azure;
using ILCDirectory.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.Main
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IILCDirectoryRepository _repo;
        private readonly ITokenizeAndSearchRepository _searchRepo;
        private readonly IConfiguration _cfg;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _logger = logger;
            _repo = repo;
            _cfg = cfg;
        }

        public IList<Person> Persons { get; set; }
        public string Search { get; set; }
        public bool LocalSearch { get; set; }
        public bool SearchAddresses { get; set; } = true;
        public bool SearchPeople { get; set; } = true;

        public async Task OnGet()
        {
            // load cookie values for search options: Search text, localSearch, searchAddresses, searchPeople
            Search = Request.Cookies["Search"];
            LocalSearch = Request.Cookies["LocalSearch"] == "true" ? true : false;
            SearchAddresses = Request.Cookies["SearchAddresses"] == "true" ? true : false;
            SearchPeople = Request.Cookies["SearchPeople"] == "true" ? true : false;

            Persons = _repo.GetAllRowsAsync<Person>(_cfg, "Person").Result;
            Persons = Persons.OrderBy(p => p.LastName).ThenBy(y => y.FirstName).ToList();
        }

        // create handler for the search button called by the ajax command on the index page
        public async Task<IActionResult> OnPostAsync()
        {
            // save search options to cookies
            Request.Form.TryGetValue("Search", out var search);

            if (string.IsNullOrEmpty(search))
                Response.Cookies.Delete("Search");
            else
                Response.Cookies.Append("Search", search);

            //Response.Cookies.Append("LocalSearch", LocalSearch.ToString());
            //Response.Cookies.Append("SearchAddresses", SearchAddresses.ToString());
            //Response.Cookies.Append("SearchPeople", SearchPeople.ToString());

            Persons = await _repo.GetAllRowsAsync<Person>(_cfg, "Person");
            return Page();
        }
    }
}