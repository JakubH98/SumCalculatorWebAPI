using SumCalculatorWebAPI.Domain;
using Microsoft.AspNetCore.Mvc;
using static SumCalculatorWebAPI.Controllers.GenericControllercs;

namespace SumCalculatorWebAPI.Controllers
{
    [Route("api/projects")]
    public class ProjectController : GenericController<Project>
    {
        public ProjectController(IRepository<Project> repository) : base(repository) { }
    }
}
