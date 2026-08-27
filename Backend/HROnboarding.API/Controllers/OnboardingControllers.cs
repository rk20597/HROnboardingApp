using Microsoft.AspNetCore.Mvc;
using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OnboardingController : ControllerBase
    {
        private readonly TeamTrackerRepository _repo;

        public OnboardingController(TeamTrackerRepository repo) 
        { 
            _repo = repo;
        }


        [HttpGet("steps")]
        public async Task<IActionResult> GetSteps() 
        {
            var steps = await _repo.GetOnboardingSteps();
            return Ok(steps);
        }

        [HttpGet("progress")]
        public async Task<IActionResult> GetProgress()
        {
            var progress = await _repo.GetOnboardingProgress();
            return Ok(progress);
        }
    }
}
