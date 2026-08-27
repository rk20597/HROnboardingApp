using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class GDLeadController : ControllerBase
    {
        private readonly TeamTrackerRepository _repo;
        public GDLeadController(TeamTrackerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo.GetGDLeadData();
            return Ok(data);
        }

    }
}
