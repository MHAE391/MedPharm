using API.DataAccess.DTOs.Attribute;

namespace API.DataAccess.DTOs.Authentication
{
    [ValidPharmacyTimes]
    public class RegisterPharmacyDTO : PharmacyDTO
    {
        
        public required string Password { get; set; }

    }
}
