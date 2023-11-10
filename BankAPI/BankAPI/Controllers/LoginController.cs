using BankAPI.Data.DTOs;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly LoginService loginService;
        private IConfiguration config;

        public LoginController(LoginService loginService, IConfiguration config)
        {
            this.loginService = loginService;
            this.config = config;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Login(AdminDto adminDto)
        {
            var admin = await loginService.GetAdmin(adminDto);

            if (admin is null)
            {
                return BadRequest(new { message = "Credenciales invalidas" });

            }
            string jwtToken = GenerateToken(admin);
            return Ok(new { token = jwtToken });
        }

        [NonAction]
        private string GenerateToken(Administrator admin)
        {
            var claims = new[] {
                new Claim(ClaimTypes.Name, admin.Name),
                new Claim(ClaimTypes.Email, admin.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("JWT:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var securityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );

            string token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return token;
        }


    }
}
