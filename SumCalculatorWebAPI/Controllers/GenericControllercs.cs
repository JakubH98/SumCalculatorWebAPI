using Microsoft.AspNetCore.Mvc;
using SumCalculatorWebAPI.Domain;

namespace SumCalculatorWebAPI.Controllers
{
    public class GenericControllercs
    {
        [ApiController]
        [Route("api/[controller]")]
        public class GenericController<T> : ControllerBase where T : class, IEntity
        {
            private readonly IRepository<T> _repository;

            public GenericController(IRepository<T> repository)
            {
                _repository = repository;
            }

            [HttpPost]
            public async virtual Task<IActionResult> Add([FromBody] T entity)
            {
                if (entity == null)
                {
                    return BadRequest("Invalid data.");
                }

                await _repository.Add(entity);
                return CreatedAtAction(nameof(GetById), new { id = entity.ID }, entity);
            }

            [HttpGet("{id}")]
            public async Task<IActionResult> GetById(int id)
            {
                var entity = await _repository.Get(id);
                if (entity == null)
                {
                    return NotFound($"Entity with ID {id} not found.");
                }
                return Ok(entity);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Update(int id, [FromBody] T updatedEntity)
            {
                var existingEntity = await _repository.Get(id);
                if (existingEntity == null)
                {
                    return NotFound($"Entity with ID {id} not found.");
                }

                updatedEntity.ID = id.ToString();
                await _repository.Update(updatedEntity);

                return Ok($"Entity with ID {id} updated successfully.");
            }

            [HttpDelete("{id}")]
            public async Task<IActionResult> Delete(int id)
            {
                var existingEntity = await _repository.Get(id);
                if (existingEntity == null)
                {
                    return NotFound($"Entity with ID {id} not found.");
                }

                await _repository.Delete(id);
                return Ok($"Entity with ID {id} deleted successfully.");
            }
        }
    }
}
