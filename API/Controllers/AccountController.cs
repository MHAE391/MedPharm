using API.DataAccess;
using API.DataAccess.Authentication;
using API.DataAccess.Context;
using API.DataAccess.DTOs;
using API.DataAccess.DTOs.Authentication;
using API.DataAccess.Models;
using Castle.Core.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IAuthenticationService authenticationService) : ControllerBase
    {

        [HttpPost("Pharmacy")]
        public async Task<ActionResult<AuthenticationResponseDTO>> RegisterPharmacy(RegisterPharmacyDTO pharmacy)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResponseDTO { Errors = ModelState.SelectMany(x => x.Value!.Errors.Select(y => y.ErrorMessage)).ToList() });
            }
            else
            {
                var response = await authenticationService.RegisterPharmacy(pharmacy);
                if (response.IsAuthenticated) return Ok(response);

                return BadRequest(response);
            }
        }
        [HttpPost("Customer")]
        public async Task<ActionResult<AuthenticationResponseDTO>> RegisterCustomer([FromBody] RegisterCustomerDTO customer) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResponseDTO { Errors = ModelState.SelectMany(x => x.Value!.Errors.Select(y => y.ErrorMessage)).ToList() });
            }
            else
            {
                var response = await authenticationService.RegisterCustomer(customer);
                if (response.IsAuthenticated) return Ok(response);

                return BadRequest(response);
            }
        }
        [HttpPost("Admin")]
        public async Task<ActionResult<AuthenticationResponseDTO>> RegisterAdmin(RegisterAdminDTO admin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthenticationResponseDTO { Errors = ModelState.SelectMany(x => x.Value!.Errors.Select(y => y.ErrorMessage)).ToList() });
            }
            else
            {
                var response = await authenticationService.RegisterAdmin(admin);
                if (response.IsAuthenticated) return Ok(response);

                return BadRequest(response);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginDTO login)
        {
            if (ModelState.IsValid)
            {
                var result = await authenticationService.Login(login);
                if(result.IsAuthenticated) return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<LoginResponseDTO>> GetCredentialsWithRefreshToken([FromBody]string refreshToken)
        {
            if (ModelState.IsValid)
            {
                var result = await authenticationService.GetCredentialsWithRefreshToken(refreshToken);
                if (result.IsAuthenticated) return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }

    }
}
