using API.DataAccess;
using API.DataAccess.Context;
using API.DataAccess.DTOs;
using API.DataAccess.DTOs.Medicine;
using API.DataAccess.Models;
using API.DataAccess.Repositories.Medicines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController(IMedicineRepository  medicinesRepository , ApplicationDbContext context) : ControllerBase
    {
        [HttpPost("MedicinePharmacy")]
        [Authorize(Roles = Setting.Pharmacy)]
        public async Task<ActionResult<MedicineResponseDTO>> AddMedicineToPharmacy(AddMedicineToPharmacyDTO medicine)
        {
            MedicineResponseDTO response = await medicinesRepository.AddMedicineToPharmacy(medicine , User.Claims);
            return response.Sucess == true ? Ok(response) : BadRequest(response);
        }


        [HttpPost("CreateMedicine")]
        [Authorize(Roles = Setting.Admin) ]
        public async Task<ActionResult<MedicineResponseDTO>> CreateMedicine(MedicineDTO medicineDTO)
        {
            MedicineResponseDTO response = await medicinesRepository.CreateNewMedicine(medicineDTO);
            return response.Sucess == true ? Ok(response) : BadRequest(response);
        }

    }
}
