namespace API.DataAccess.DTOs.Medicine
{
    public class AddMedicineToPharmacyDTO
    {
        public required string MedicineId { get; set; }
        public int Quntity { get; set; }
        public double Price { get; set; }
    }
}
