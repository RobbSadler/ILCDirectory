﻿using ILCDirectory.Data.Repositories;

namespace ILCDirectory.Pages.Account
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IILCDirectoryRepository _repo;
        private readonly IConfiguration _cfg;
        public IList<Address> Addresses { get;set; }

        public IndexModel(ILogger<IndexModel> logger, IILCDirectoryRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task OnGetAsync()
        {
            Addresses = await _repo.GetAllRowsAsync<Address>("Address");
        }
    }
}
