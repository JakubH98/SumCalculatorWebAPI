using SumCalculatorWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using static SumCalculatorWebAPI.Controllers.GenericControllercs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace SumCalculatorWebAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Program.AuthScheme)]
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
        [AllowAnonymous]
        [HttpPost("login")]
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

            await HttpContext.SignInAsync(Program.AuthScheme, new System.Security.Claims.ClaimsPrincipal(
            new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, request.Username)
            }, Program.AuthScheme)));

            existingUser.Password = null; // Remove the password from the response
            return Ok(existingUser);

        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Program.AuthScheme);
            return Ok("Logged out successfully.");
        }
    }
}
