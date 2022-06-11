//using Microsoft.EntityFrameworkCore;
//using ILCDirectory.Data;
using ILCDirectory.Models;
using Microsoft.AspNetCore.Authorization;

namespace ILCDirectory.Views.People
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IPersonRepository _personRepo;

        public IndexModel(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
        }

        public IList<Person> Persons { get; set; }
        public Family Family { get; set; }

        public async Task OnGetAsync()
        {
            Persons = (IList<Person>)(await _personRepo.GetAllAsync());
        }
    }
}
