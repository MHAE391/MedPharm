using System.Text.Json.Serialization;

namespace API.DataAccess.DTOs.Authentication
{
    public class AuthenticationResponseDTO
    {
        public bool IsAuthenticated { get; set; }
        public string? UserName { get; set; } = null;
        public string? Email { get; set; } = null;
        public string? Token { get; set; } = null;
        public DateTime? TokenExpiresOn { get; set; } = null;
        public IEnumerable<string> Errors { get; set; } = Enumerable.Empty<string>();

        public string? RefreshToken { get; set; } = null;

        public DateTime? RefreshTokenExpiration { get; set; } = null;
        
    }
}
