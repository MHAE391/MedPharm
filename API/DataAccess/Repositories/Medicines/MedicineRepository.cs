using API.DataAccess.Context;
using API.DataAccess.DTOs;
using API.DataAccess.DTOs.Medicine;
using API.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.DataAccess.Repositories.Medicines
{
    public class MedicineRepository(ApplicationDbContext context) : IMedicineRepository
    {
        public async Task<MedicineResponseDTO> AddMedicineToPharmacy(AddMedicineToPharmacyDTO medicine, IEnumerable<Claim> pharmacyClaims)
        {
            string pharmacy = pharmacyClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

            var storedMedicine = await context.Medicines.FirstOrDefaultAsync(x => x.Id == medicine.MedicineId);
            if (storedMedicine is null )
                return new MedicineResponseDTO { Sucess = false, Errors = new List<string> { "No Medicine Founded." } };


            var isStored = await context.MedicinePharmacies.FirstOrDefaultAsync(x => x.PharmacyId == pharmacy && x.MedicineId == medicine.MedicineId);
            if (isStored is null) 
            {
                var medicinePharmacy = new MedicinePharmacy { 
                    MedicineId = medicine.MedicineId,
                    PharmacyId = pharmacy,
                    Quntity = medicine.Quntity,
                    Price = medicine.Price
                };
                await context.MedicinePharmacies.AddAsync(medicinePharmacy);
                await context.SaveChangesAsync();
                return new MedicineResponseDTO { Sucess = true };
            }
            return new MedicineResponseDTO { Sucess = false, Errors = new List<string> { "Medicine Is Already Add To The Pharmacy" } };

        }

        public async Task<MedicineResponseDTO> CreateNewMedicine(MedicineDTO medicine)
        {
            var isStored = await context.Medicines.FirstOrDefaultAsync(x => x.Name == medicine.Name);
            if( isStored is null)
            {
                var newMedicine = new Medicine { 
                    Name = medicine.Name  ,
                    Description  = medicine.Description ,
                    ImageUrl = medicine.ImageURL  }; 
                await context.Medicines.AddAsync (newMedicine);
                await context.SaveChangesAsync();
                return new MedicineResponseDTO { Sucess = true };
            }
            return new MedicineResponseDTO { Sucess = false, Errors = new List<string> { "Medicine Is Already Stored" } };
        }
    }
}
