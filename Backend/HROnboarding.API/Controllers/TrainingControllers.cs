using Microsoft.AspNetCore.Mvc;
using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingControllers : ControllerBase
    {
        private readonly ExcelRepository _repo;

        public TrainingControllers(ExcelRepository repo) 
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var training = await _repo.GetAllTraining();
            return Ok(training);
        }
    }
}
