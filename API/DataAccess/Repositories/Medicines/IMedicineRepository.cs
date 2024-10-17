using API.DataAccess.DTOs;
using API.DataAccess.DTOs.Medicine;
using System.Security.Claims;

namespace API.DataAccess.Repositories.Medicines
{
    public interface IMedicineRepository
    {
        public Task<MedicineResponseDTO> CreateNewMedicine(MedicineDTO medicine);
        public Task<MedicineResponseDTO> AddMedicineToPharmacy(AddMedicineToPharmacyDTO medicine , IEnumerable<Claim> pharmacyClaims);
    }

}
