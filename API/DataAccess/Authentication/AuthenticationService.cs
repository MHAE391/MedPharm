using API.DataAccess.Context;
using API.DataAccess.DTOs.Authentication;
using API.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.DataAccess.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWT _jwt;

        public AuthenticationService(UserManager<ApplicationUser> userManager, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO login)
        {
            var response = new LoginResponseDTO();
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user is null || !(await _userManager.CheckPasswordAsync( user, login.Password)))
            {
                response.Errors = new List<string> { "Email or Password is wrong" };
                return response;
            }

            response.IsAuthenticated = true;
            var token = await GenerateToken(user);
            response.Token = token.Token;
            response.TokenExpiresOn = token.ExpiresOn;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.IsPharmacy = user.IsPharm; 
            response.IsAdmin = user.IsAdmin;
            if(user.RefreshTokens.Any(t => t.IsActive))
            {
                var activeToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                response.RefreshToken = activeToken!.Token;
                response.RefreshTokenExpiration = activeToken.ExpiresOn;
            }
            else
            {
                var newRefreshToken = GenerateRefreshToken();
                response.RefreshToken = newRefreshToken.Token;
                response.RefreshTokenExpiration = newRefreshToken.ExpiresOn;
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
            } 
            return response;
        }

        public async Task<AuthenticationResponseDTO> RegisterCustomer(RegisterCustomerDTO registerCustomer)
        {
            var response = new AuthenticationResponseDTO();
            if (await _userManager.FindByEmailAsync(registerCustomer.CustomerEmail) is not null)
            {
                response.Errors = new List<string> { $" '{registerCustomer.CustomerEmail}' is already used !!" };
                return response;
            }

            if (await _userManager.FindByNameAsync(registerCustomer.CustomerName) is not null)
            {
                response.Errors = new List<string> { $" '{registerCustomer.CustomerName}' is already used !!" };
                return response;

            }

            var newCustomer = new Customer { 
                DateOfBarth = registerCustomer.DateOfBarth,
                Latitude = registerCustomer.Latitude,
                Longitude = registerCustomer.Longitude
            };

            var user = new ApplicationUser
            {
                UserName = registerCustomer.CustomerName,
                Email = registerCustomer.CustomerEmail,
                IsPharm = false,
                Customer = newCustomer,
                IsAdmin = false
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerCustomer.Password);
                if (result.Succeeded)
                {
                    response.IsAuthenticated = true;
                    var role = await _userManager.AddToRoleAsync(user, Setting.Customer);
                    var token = await GenerateToken(user);
                    response.Token = token.Token;
                    response.TokenExpiresOn = token.ExpiresOn;
                    response.Email = user.Email;
                    response.UserName = user.UserName;
                    return response;
                }
                else
                {
                    response.Errors = result.Errors.Select(x => x.Description).ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Errors = new List<string> { ex.Message };
                return response;
            }
        }

        public async Task<AuthenticationResponseDTO> RegisterPharmacy(RegisterPharmacyDTO registerPharmacy)
        {
            var response = new AuthenticationResponseDTO();
            if(await _userManager.FindByEmailAsync(registerPharmacy.PharmacyEmail) is not null)
            {
                response.Errors = new List<string> { $" '{registerPharmacy.PharmacyEmail}' is already used !!"};
                return response;

            }

            if (await _userManager.FindByNameAsync(registerPharmacy.PharmacyName) is not null)
            {
                response.Errors = new List<string> { $" '{registerPharmacy.PharmacyName}' is already used !!" };
                return response;

            }
            var newPharmacy = new Pharmacy { 
                IsOpenAllTime = registerPharmacy.IsOpenAllTime,
                OpenTime = registerPharmacy.OpenTime,
                CloseTime = registerPharmacy.CloseTime,
                Latitude = registerPharmacy.Latitude,
                Longitude = registerPharmacy.Longitude
            };

            var user = new ApplicationUser
            {
                UserName = registerPharmacy.PharmacyName,
                Email = registerPharmacy.PharmacyEmail,
                IsPharm = true,
                Pharmacy = newPharmacy,
                IsAdmin = false 

            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerPharmacy.Password);
                if (result.Succeeded)
                {
                    response.IsAuthenticated = true;
                    var role = await _userManager.AddToRoleAsync(user, Setting.Pharmacy);
                    var token  = await GenerateToken(user);
                    response.Token = token.Token;
                    response.TokenExpiresOn = token.ExpiresOn;
                    response.Email = user.Email;
                    response.UserName = user.UserName;
                    return response;

                }
                else
                {
                    response.Errors = result.Errors.Select(x => x.Description).ToList();
                    return response;

                }
            }
            catch (Exception ex)
            {
                response.Errors = new List<string> {  ex.Message };
                return response;
            }
        }

        private async Task<JwtToken> GenerateToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.Duration),
                signingCredentials: signingCredentials);

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                return new JwtToken { Token  = token  , ExpiresOn = jwtSecurityToken.ValidTo};
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken 
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(1),
                CreatedOn = DateTime.UtcNow
            };
        }

        public async Task<LoginResponseDTO> GetCredentialsWithRefreshToken(string refreshToken)
        {
            var response = new LoginResponseDTO();
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if(user is null)
            {
                response.Errors = new List<string>() { "Invalid Token" };
                return response;
            }
            var oldRefreshToken = user.RefreshTokens.First(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
            {
                response.Errors = new List<string>() { "Inactive Token" };
                return response;
            }

            response.IsAuthenticated = true;
            var token = await GenerateToken(user);
            response.Token = token.Token;
            response.TokenExpiresOn = token.ExpiresOn;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.IsPharmacy = user.IsPharm;
            response.RefreshToken = oldRefreshToken.Token;
            response.IsAdmin = user.IsAdmin;
            response.RefreshTokenExpiration = oldRefreshToken.ExpiresOn;
            return response;
        }

        public async Task<AuthenticationResponseDTO> RegisterAdmin(RegisterAdminDTO registerAdmin)
        {
            var response = new AuthenticationResponseDTO();
            if (await _userManager.FindByEmailAsync(registerAdmin.AdminEmail) is not null)
            {
                response.Errors = new List<string> { $" '{registerAdmin.AdminEmail}' is already used !!" };
                return response;
            }

            if (await _userManager.FindByNameAsync(registerAdmin.AdminName) is not null)
            {
                response.Errors = new List<string> { $" '{registerAdmin.AdminName}' is already used !!" };
                return response;

            }
            var user = new ApplicationUser
            {
                UserName = registerAdmin.AdminName,
                Email = registerAdmin.AdminEmail,
                IsPharm = false,
                IsAdmin = true
            };
            try
            {
                var result = await _userManager.CreateAsync(user, registerAdmin.Password);
                if (result.Succeeded)
                {
                    response.IsAuthenticated = true;
                    var role = await _userManager.AddToRoleAsync(user, Setting.Admin);
                    var token = await GenerateToken(user);
                    response.Token = token.Token;
                    response.TokenExpiresOn = token.ExpiresOn;
                    response.Email = user.Email;
                    response.UserName = user.UserName;
                    return response;
                }
                else
                {
                    response.Errors = result.Errors.Select(x => x.Description).ToList();
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Errors = new List<string> { ex.Message };
                return response;
            }
        }
    }


    
}
