using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ILCDirectory.Data;
using ILCDirectory.Models;

namespace ILCDirectory.Views.People
{
    public class CreateModel : PageModel
    {
        private readonly IPersonRepository _personRepo;
        
        public CreateModel(IPersonRepository personRepo)
        {
            _personRepo = personRepo;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Person Person { get; set; }


        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _personRepo.InsertAsync(Person);

            return RedirectToPage("./Index");
        }
    }
}
