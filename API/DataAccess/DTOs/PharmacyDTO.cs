using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.DTOs
{
    public class PharmacyDTO
    {
        [Required]
        public string PharmacyName { get; set; }
        [EmailAddress , Required]
        public string PharmacyEmail { get; set; }
        [Required]
        public bool IsOpenAllTime { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        [Required]
        public double Longitude { get; set; }
        [Required]
        public double Latitude { get; set; }

    }
}
