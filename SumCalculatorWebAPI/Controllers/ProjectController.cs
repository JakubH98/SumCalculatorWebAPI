using SumCalculatorWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;

namespace SumCalculatorWebAPI.Controllers
{
    [Route("api/projects")]
    public class ProjectController : GenericController<Project>
    {
        public ProjectController(IRepository<Project> repository) : base(repository) { }
    }
}
