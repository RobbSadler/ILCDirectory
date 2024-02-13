using Azure;
using ILCDirectory.Authentication;
using ILCDirectory.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.Main
{
    [Authorize]
    [BindProperties]
    [ValidateAntiForgeryToken]
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
        public PersonFamilyDetails PersonFamilyDetails { get; set; }
        public PersonAddressDetails PersonAddressDetails { get; set; }
        public PersonFamilyAddressDetails PersonFamilyAddressDetails { get; set; }

        public string SearchText { get; set; }
        public bool LocalSearch { get; set; }
        public bool SearchAddresses { get; set; } = true;
        public bool IncludeChildren { get; set; } = true;
        public bool SearchPartialWords { get; set; } = true;

        public async Task OnGetAsync()
        {
            // load cookie values for search options: Search text, localSearch, searchAddresses, searchPeople
            SearchText = Request.Cookies["SearchText"];
            LocalSearch = Request.Cookies["LocalSearch"] == "true" ? true : false;
            IncludeChildren = Request.Cookies["IncludeChildren"] == "true" ? true : false;
            SearchPartialWords = Request.Cookies["SearchPartialWords"] == "true" ? true : false;

            Persons = await SearchAsync(LocalSearch, IncludeChildren, SearchPartialWords, SearchText);
        }

        public async Task<IActionResult> OnPostSearchAsync([FromForm] bool localSearch, [FromForm] bool includeChildren,
            [FromForm] bool searchPartialWords, [FromForm] string searchText)
        {
            Response.Cookies.Delete("SearchText");

            if(!string.IsNullOrEmpty(searchText))
                Response.Cookies.Append("SearchText", searchText);

            Response.Cookies.Delete("LocalSearch");
            LocalSearch = localSearch;
            Response.Cookies.Append("LocalSearch", localSearch.ToString());
            Response.Cookies.Delete("IncludeChildren");
            IncludeChildren = includeChildren;
            Response.Cookies.Append("IncludeChildren", includeChildren.ToString());
            Response.Cookies.Delete("SearchPartialWords");
            SearchPartialWords = searchPartialWords;
            Response.Cookies.Append("SearchPartialWords", searchPartialWords.ToString());

            Persons = await SearchAsync(localSearch, includeChildren, searchPartialWords, searchText);

            return Page();
        }

        public async Task<IActionResult> OnPostPersonFamilyAddressDetailsAsync([FromForm] int personId, [FromForm] int? spousePersonId)
        {
            PersonFamilyAddressDetails = await _repo.GetPersonFamilyAddressDetailsAsync(personId, spousePersonId);
            return Partial("_centerPersonDetails", PersonFamilyAddressDetails);
        }

        public async Task<IActionResult> OnPostPersonFamilyDetailsAsync([FromForm] int personId, [FromForm] int? spousePersonId)
        {
            PersonFamilyDetails = await _repo.GetPersonFamilyDetailsAsync(personId, spousePersonId);
            return Partial("_rightPane", PersonFamilyDetails);
        }

        public async Task<IActionResult> OnPostPersonAddressDetailsAsync([FromForm] int personId)
        {
            PersonAddressDetails = await _repo.GetPersonAddressDetailsAsync(personId);
            return Partial("_leftPane", PersonAddressDetails);
        }

        public async Task<IActionResult> OnPostCreatePersonAsync([FromForm] bool localSearch, [FromForm] bool searchAddresses, [FromForm] bool searchPeople, [FromForm] string searchText)
        {
            return Page();
        }

        private async Task<IList<Person>> SearchAsync(bool localSearch, bool includeChildren, bool searchPartialWords, string searchText)
        {
            IList<Person> persons;
            persons = await _searchRepo.SearchForPersonOrAddress(_cfg, searchText, searchPartialWords, includeChildren, localSearch);

            return persons;
        }
    }
}
