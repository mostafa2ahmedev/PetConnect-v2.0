using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetConnect.BLL.Services.DTOs.Account;
using PetConnect.BLL.Services.Interfaces;
using PetConnect.DAL.Data.Identity;

namespace PetConnect.BLL.Services.Classes
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;

        public JwtService(IConfiguration _configuration , UserManager<ApplicationUser> _userManager)
        {
            configuration = _configuration;
            userManager = _userManager;
        }

        public async Task<AuthResponse> CreateJwtToken(ApplicationUser user)
        {

            var roles = await userManager.GetRolesAsync(user);

            List<Claim> claims = new List<Claim>();
      
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken securityTokenGenerator = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"], claims:claims, expires: DateTime.Now.AddHours(1), signingCredentials: signingCredentials);

            JwtSecurityTokenHandler jwt = new JwtSecurityTokenHandler();

            string token = jwt.WriteToken(securityTokenGenerator);

            return new AuthResponse() { 
                Token = token , 
                Email = user.Email , 
                //Expiration = expiration,
                Username = user.UserName,
                Id = user.Id,
                Roles = roles.ToList()
            };
        }


    }
}
