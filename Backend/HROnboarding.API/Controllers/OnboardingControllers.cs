using HROnboarding.API.Models;
using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetStepsByTeam(string teamName) 
        {
            var steps = await _repo.GetOnboardingStepsByTeam(teamName);
            return Ok(steps);
        }

        [HttpGet("progress")]
        public async Task<IActionResult> GetProgress()
        {
            var progress = await _repo.GetOnboardingProgress();
            return Ok(progress);
        }

        [HttpPost("steps")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStep(
    [FromBody] OnboardingStep step)
        {
            await _repo.AddOnboardingStep(step);
            return Ok(new { message = "Step added" });
        }

        [HttpPatch("steps/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStep(
            int id,
            [FromBody] OnboardingStep step)
        {
            step.StepID = id;
            await _repo.UpdateOnboardingStep(step);
            return Ok(new { message = "Step updated" });
        }

        [HttpDelete("steps/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStep(
            int id)
        {
            await _repo.DeleteOnboardingStep(id);
            return Ok(new { message = "Step deleted" });
        }

    }
}
