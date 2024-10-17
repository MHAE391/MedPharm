using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.DTOs
{
    public class CustomerDTO
    {
        [Required]
        public string CustomerName { get; set; }
        [EmailAddress, Required]
        public string CustomerEmail { get; set; }
        public DateTime DateOfBarth { get; set; }

        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

    }
}
