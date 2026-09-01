using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using HROnboarding.API.Repositories;
using HROnboarding.API.Models;

namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingStatusController : ControllerBase
    {
        private readonly TeamTrackerRepository _repo;

        public TrainingStatusController(
            TeamTrackerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _repo
                .GetTrainingStatus();
            return Ok(data);
        }

        [HttpGet("candidate/{id}")]
        public async Task<IActionResult>
            GetByCandidate(int id)
        {
            var all = await _repo
                .GetTrainingStatus();
            var data = all.Where(t =>
                t.CandidateID == id).ToList();
            return Ok(data);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Add(
            [FromBody] TrainingStatus training)
        {
            await _repo.AddTrainingStatus(training);
            return Ok(new
            {
                message = "Training added"
            });
        }

        [HttpPatch("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] TrainingStatus training)
        {
            training.TrainingID = id;
            await _repo.UpdateTrainingStatus(
                training);
            return Ok(new
            {
                message = "Training updated"
            });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(
            int id)
        {
            await _repo.DeleteTrainingStatus(id);
            return Ok(new
            {
                message = "Training deleted"
            });
        }
    }
}

