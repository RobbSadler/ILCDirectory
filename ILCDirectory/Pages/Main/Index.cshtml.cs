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

        public IndexModel(ILogger<IndexModel> logger, IConfiguration cfg, IILCDirectoryRepository repo, ITokenizeAndSearchRepository searchRepo)
        {
            _logger = logger;
            _repo = repo;
            _cfg = cfg;
            _searchRepo = searchRepo;
        }

        public IList<Person> Persons { get; set; }
        public string SearchText { get; set; }
        public bool LocalSearch { get; set; }
        public bool SearchAddresses { get; set; } = true;
        public bool SearchPeople { get; set; } = true;

        public async Task OnGet()
        {
            // load cookie values for search options: Search text, localSearch, searchAddresses, searchPeople
            SearchText = Request.Cookies["Search"];
            LocalSearch = Request.Cookies["LocalSearch"] == "true" ? true : false;
            SearchAddresses = Request.Cookies["SearchAddresses"] == "true" ? true : false;
            SearchPeople = Request.Cookies["SearchPeople"] == "true" ? true : false;

            Persons = _repo.GetAllRowsAsync<Person>(_cfg, "Person").Result;
            Persons = Persons.OrderBy(p => p.LastName).ThenBy(y => y.FirstName).ToList();
        }

        public async Task<IActionResult> OnPostAsync([FromForm] bool localSearch, [FromForm] bool searchAddresses, [FromForm] bool searchPeople, [FromForm] string searchText)
        {
            // save search options to cookies
            if (string.IsNullOrEmpty(searchText))
                Response.Cookies.Delete("SearchText");
            else
                Response.Cookies.Append("SearchText", searchText);

            Response.Cookies.Delete("LocalSearch");
            Response.Cookies.Append("LocalSearch", LocalSearch.ToString());
            Response.Cookies.Delete("SearchAddresses");
            Response.Cookies.Append("SearchAddresses", SearchAddresses.ToString());
            Response.Cookies.Delete("SearchPeople");
            Response.Cookies.Append("SearchPeople", SearchPeople.ToString());

            IList<Person> persons;
            if (string.IsNullOrEmpty(searchText))
                persons = await _repo.GetAllRowsAsync<Person>(_cfg, "Person");
            else 
                (persons, var addresses) = await _searchRepo.SearchForPersonOrAddress(_cfg, searchText);

            Persons = persons.OrderByDescending(p => p.LastName).ThenBy(y => y.FirstName).ToList();
            return Page();
        }
    }
}