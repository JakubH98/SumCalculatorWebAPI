using SumCalculatorWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using static SumCalculatorWebAPI.Controllers.GenericControllercs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace SumCalculatorWebAPI.Controllers
{
    [Route("api/users")]
    public class UsersController : GenericController<User>
    {
        private readonly IRepository<User> _repository;

        public UsersController(IRepository<User> repository) : base(repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public override async Task<IActionResult> Add([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Username and Password are required.");
            }
            bool usernameExists = await _repository.Exists(u => u.Username == user.Username);
            if (usernameExists)
            {
                return Conflict(new { message = $"A user with the username '{user.Username}' already exists." });
            }

            bool emailExists = await _repository.Exists(u => u.Email == user.Email);
            if (emailExists)
            {
                return Conflict(new { message = $"A user with the email {user.Email} already exists. Please use a different one" });
            }

            await _repository.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.ID }, user);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest("Username and Password are required.");
            }


            var existingUser = await _repository.GetFirstOrDefault(u => u.Username == request.Username && u.Password == request.Password);

            if (existingUser == null)
            {
                return Conflict("Invalid username or password.");
            }
            var token = GenerateJwtToken(existingUser.Username);


            existingUser.Password = null;

            return Ok(new
            {
                Token = token,
                User = existingUser
            });
        }

        private string GenerateJwtToken(string username)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JakubsTopSecretSuperKeyThatNobodyShouldEverKnowAbout12345"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: "https://localhost:7214",
                audience: "https://localhost:7214",
                claims: new[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.UtcNow.AddMinutes(20),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        [HttpPost("logout")]
        [Authorize] // Ensures the user is authenticated to access this endpoint
        public IActionResult Logout()
        {
            // JWT is stateless, so the backend cannot invalidate it directly.
            // The frontend should delete the token locally (e.g., from localStorage or cookies).
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
