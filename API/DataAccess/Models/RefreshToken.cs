using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace API.DataAccess.Models
{
    [Owned]
    public class RefreshToken
    {
        public required string Token { get; set; }

        public DateTime ExpiresOn { get; set; }

        public bool IsExpired  => DateTime.UtcNow >= ExpiresOn;

        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }

        public bool IsActive => RevokedOn is null && !IsExpired;    
    }
}
