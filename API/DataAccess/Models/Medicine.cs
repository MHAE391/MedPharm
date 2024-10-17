using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace API.DataAccess.Models
{
    public class Medicine
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }        
        public string ImageUrl { get; set; }

        public virtual IEnumerable<MedicinePharmacy> MedicinePharmacies { get; set; }
    }
}
