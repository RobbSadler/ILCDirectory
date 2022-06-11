using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ILCDirectory.Data;
using ILCDirectory.Models;

namespace ILCDirectory.Views.People
{
    public class DetailsModel : PageModel
    {
        private readonly IPersonRepository _personRepo;

        public DetailsModel(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
        }

        public Person Person { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Person = await _personRepo.GetAsync(id);

            if (Person == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
