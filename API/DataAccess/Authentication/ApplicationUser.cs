using API.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataAccess.Authentication
{
    public class ApplicationUser : IdentityUser
    {

        public bool IsPharm { get; set; }
        public bool IsAdmin { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Pharmacy? Pharmacy { get; set; }

        public List<RefreshToken>? RefreshTokens { get; set; }

    }
}
