using SumCalculatorWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using static SumCalculatorWebAPI.Controllers.GenericControllercs;

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

            bool usernameExists = await _repository.ExistsAsync(u => u.Username == user.Username);
            if (usernameExists)
            {
                return Conflict($"A user with the username '{user.Username}' already exists.");
            }

            bool emailExists = await _repository.ExistsAsync(u => u.Email == user.Email);
            if (emailExists)
            {
                return Conflict($"A user with the email {user.Email} already exists. Please use a different one");
            }

            await _repository.Add(user);
            return CreatedAtAction(nameof(GetById), new { id = user.ID }, user);
        }
    }
}
