using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using book_store_be.Models;
using Microsoft.Extensions.Configuration;
using book_store_be.Repositories;

namespace book_store_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        // POST: api/SignUp
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp([FromBody] SignUpModel signUpModel)
        {
            if (signUpModel == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var userExists = await _authRepository.UserExistsAsync(signUpModel.Email);
            if (userExists)
                return BadRequest(new { Message = "User already exists!" });

            var user = new UserModel
            {
                UserName = signUpModel.UserName,
                Email = signUpModel.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(signUpModel.Password),
                IsAdmin = false
            };

            await _authRepository.AddUserAsync(user);

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // POST: api/SignIn
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInModel signInModel)
        {
            if (signInModel == null || !ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authRepository.GetUserByEmailAsync(signInModel.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(signInModel.Password, user.Password))
                return Unauthorized(new { Message = "Invalid email or password." });

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        // POST: api/Logout
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var user = await _authRepository.GetUserByUserNameAsync("אורח");
            if (user == null)
            {
                return NotFound(new { Message = "User not found." });
            }

            var token = GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        private string GenerateJwtToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
