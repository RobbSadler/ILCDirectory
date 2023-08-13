using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILCDirectory.Pages.Main
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IBuildingRepository _buildingRepository;
        //private readonly ICityCodeRepository _cityCodeRepository;
        private readonly IClassificationRepository _classificationRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IFamilyRepository _familyRepository;
        private readonly IMailDeliveryRepository _mailDeliveryRepository;
        private readonly IOtherMailRepository _otherMailRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IWoRepository _woRepository;
        private readonly IWorkgroupRepository _workgroupRepository;


        public IndexModel(
            IAddressRepository addressRepository,
            IBuildingRepository buildingRepository,
            //ICityCodeRepository cityCodeRepository,
            IClassificationRepository classificationRepository,
            IEmailRepository emailRepository,
            IFamilyRepository familyRepository,
            IMailDeliveryRepository mailDeliveryRepository,
            IOtherMailRepository otherMailRepository,
            IPersonRepository personRepository,
            IVehicleRepository vehicleRepository,
            IWoRepository woRepository,
            IWorkgroupRepository workgroupRepository
            )
        {
            _addressRepository = addressRepository;
            _buildingRepository = buildingRepository;
            //_cityCodeRepository = cityCodeRepository;
            _classificationRepository = classificationRepository;
            _emailRepository = emailRepository;
            _familyRepository = familyRepository;
            _mailDeliveryRepository = mailDeliveryRepository;
            _otherMailRepository = otherMailRepository;
            _personRepository = personRepository;
            _vehicleRepository = vehicleRepository;
            _woRepository = woRepository;
            _workgroupRepository = workgroupRepository;
        }

        public IList<Person> Persons { get; set; }
        public Family Family { get; set; }

        public async Task OnGetAsync()
        {
            Persons = (IList<Person>)(await _personRepository.GetAllAsync());
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task OnPostAsync()
        {
            RedirectToPage("/Main");
        }
    }
}
