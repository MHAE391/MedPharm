namespace API.DataAccess.Models
{
    public class MedicinePharmacy
    {
        public string MedicineId { get; set; }
        public string PharmacyId { get; set; }
        public int Quntity { get; set; }
        public double Price { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }
        public virtual Medicine Medicine { get; set; }
    }
}
