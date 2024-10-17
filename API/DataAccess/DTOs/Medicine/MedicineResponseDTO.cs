namespace API.DataAccess.DTOs.Medicine
{
    public class MedicineResponseDTO
    {
        public bool? Sucess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
