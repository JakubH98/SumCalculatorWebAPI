using SumCalculatorWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace SumCalculatorWebAPI.Controllers
{
    [Route("api/users")]
    public class UserController : GenericController<User>
    {
        public UserController(IRepository<User> repository) : base(repository) { }
    }
}
