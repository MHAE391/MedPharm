namespace API.DataAccess.DTOs.Authentication
{
    public class JwtToken
    {
        public required string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
