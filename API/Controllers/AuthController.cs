using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravelSuggest.Business;
using TravelSuggest.Models;

namespace TravelSuggest.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _configuration;

        public AuthController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginDTO userDto)
        {

            if (string.IsNullOrEmpty(userDto.UserName) || string.IsNullOrEmpty(userDto.Password))
            {
                return BadRequest("Nombre de usuario y contraseña son obligatorios.");
            }

            var user = _userService.Authenticate(userDto.UserName, userDto.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            // Acceder a la configuración a través de IConfiguration
            var secretKey = _configuration["JwtSettings:Secret"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("La clave secreta de JWT no está configurada.");
            }

            var key = Encoding.UTF8.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}