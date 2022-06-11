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
    public class IndexModel : PageModel
    {
        private readonly IAddressRepository _addressRepo;
        public IList<Address> Addresses { get;set; }

        public IndexModel(IAddressRepository addressRepo)
        {
            _addressRepo = addressRepo;
        }

        public async Task OnGetAsync()
        {
            Addresses = (IList<Address>)await _addressRepo.GetAllAsync();
        }
    }
}
