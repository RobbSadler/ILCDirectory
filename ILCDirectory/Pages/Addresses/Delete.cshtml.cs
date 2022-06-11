#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ILCDirectory.Data;
using ILCDirectory.Models;

namespace ILCDirectory.Pages.Addresses
{
    public class DeleteModel : PageModel
    {
        private readonly IAddressRepository _addressRepo;
        private readonly IConfiguration _configuration;

        public DeleteModel(IConfiguration configuration, IAddressRepository addressRepo)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _addressRepo.GetAsync(id);

            if (Address != null)
            {
                await _addressRepo.DeleteAsync(Address.AddressId);
            }

            return RedirectToPage("./Index");
        }
    }
}
