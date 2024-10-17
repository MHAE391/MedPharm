using Microsoft.EntityFrameworkCore.Query.Internal;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.DTOs.Authentication
{
    public class RegisterAdminDTO
    {
        public required string  AdminName { get; set; }
        [EmailAddress]
        public required string AdminEmail { get; set; }
        public required string Password { get; set; }

    }
}
