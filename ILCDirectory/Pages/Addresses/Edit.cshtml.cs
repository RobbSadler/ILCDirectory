using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ILCDirectory.Data;
using ILCDirectory.Models;

namespace ILCDirectory.Pages.Addresses
{
    public class EditModel : PageModel
    {
        private readonly IAddressRepository _addressRepo;
        private readonly IConfiguration _configuration;

        public EditModel(IConfiguration configuration, IAddressRepository addressRepo)
        {
            _addressRepo = addressRepo;
            _configuration = configuration;
        }

        [BindProperty]
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _addressRepo.GetAsync(id);

            if (Address == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _addressRepo.UpdateAsync(Address);

            return RedirectToPage("./Index");
        }

        //private bool AddressExists(int id)
        //{
        //    return (await _addressRepo.GetAsync(id)) == null;
        //}
    }
}
