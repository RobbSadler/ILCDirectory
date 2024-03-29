﻿namespace ILCDirectory.Pages.Addresses
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly IILCDirectoryRepository _repo;

        public EditModel(IConfiguration cfg, IILCDirectoryRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Address Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Address = await _repo.GetRowByIdAsync<Address>((int)id, "Address");

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

            //await _repo.UpdateAddressAsync(_cfg,Address);

            return RedirectToPage("./Index");
        }

        //private bool AddressExists(int id)
        //{
        //    return (await _addressRepo.GetAsync(id)) == null;
        //}
    }
}
