using API.DataAccess.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataAccess.Models
{
    public class Customer
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
        public DateTime DateOfBarth { get; set; }
        [Key]
        public string Id { get; set; }

        [InverseProperty(nameof(ApplicationUser.Customer))]
        public virtual ApplicationUser User { get; set; }

    }
}
