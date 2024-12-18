using Microsoft.AspNetCore.Mvc;
using SumCalculatorWebAPI.Domain;
using static SumCalculatorWebAPI.Controllers.GenericControllercs;

namespace SumCalculatorWebAPI.Controllers
{
    public class MethodController : GenericController<Method>
    {
        private readonly IRepository<Method> _repository;

        public MethodController(IRepository<Method> repository) : base(repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public override async Task<IActionResult> Add([FromBody] Method method)
        {
            if (method == null || string.IsNullOrWhiteSpace(method.Title))
            {
                return BadRequest("Method title is required.");
            }

            await _repository.Add(method);
            return CreatedAtAction(nameof(GetById), new { id = method.ID }, method);
        }

        [HttpGet("{id}")]
        public override async Task<IActionResult> GetById(int id)
        {
            var method = await _repository.Get(id);
            if (method == null)
            {
                return NotFound($"Method with ID {id} not found.");
            }
            return Ok(method);
        }

    }
}
