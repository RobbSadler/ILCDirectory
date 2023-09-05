﻿namespace ILCDirectory.Pages.People
{
    [Authorize]
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
        public Family Family { get; set; }

        public async Task OnGetAsync()
        {
            Persons = await _repo.GetAllPersonsAsync(_cfg);
        }
    }
}
