using API.DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        [HttpGet("MedicinePharmacy")]
        [Authorize(Roles = Setting.Pharmacy)]
        public ActionResult<string> AddMedicineToPharmacy()
        {
            return Ok(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

    }
}
