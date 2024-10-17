using API.DataAccess.DTOs.Authentication;

namespace API.DataAccess.Authentication
{
    public interface IAuthenticationService
    {
        public Task<AuthenticationResponseDTO> RegisterPharmacy(RegisterPharmacyDTO registerPharmacy) ;

        public Task<LoginResponseDTO> Login(LoginDTO login);

        public Task<AuthenticationResponseDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer);

        public Task<AuthenticationResponseDTO> RegisterAdmin(RegisterAdminDTO registerAdmin);

        public Task<LoginResponseDTO> GetCredentialsWithRefreshToken(string refreshToken);
    }
}
