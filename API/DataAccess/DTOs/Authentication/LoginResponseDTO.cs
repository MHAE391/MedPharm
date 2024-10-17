namespace API.DataAccess.DTOs.Authentication
{
    public class LoginResponseDTO : AuthenticationResponseDTO
    {
        public bool? IsPharmacy { get; set; } = null;
        public bool? IsAdmin { get; set; } = null;
    }
}
