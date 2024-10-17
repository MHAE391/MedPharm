using API.DataAccess.Authentication;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.DataAccess.Models
{
    public class Pharmacy
    {
        public double Longitude { get; set; }

        public double Latitude { get; set; }
        public bool IsOpenAllTime { get; set; }
        public DateTime? OpenTime { get; set; }
        public DateTime? CloseTime { get; set; }
        
        [Key]
        public string Id { get; set; }
        [InverseProperty(nameof(ApplicationUser.Pharmacy))]
        public virtual ApplicationUser User { get; set; }

        public virtual IEnumerable<MedicinePharmacy> PharmacyMedicines { get; set; }


    }
}
