using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TrainingController : ControllerBase
    {
        private readonly TeamTrackerRepository _repo;
        public TrainingController(TeamTrackerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("courses/{domain}")]
        public async Task<IActionResult> GetCourses(string domain)
        {
            var validDomains = new[] { "Copilot", "AKS", "ROVO", "Datadog" };
            if (!validDomains.Contains(domain))
            {
                return BadRequest("Invalid Domain");
            }

            var courses = await _repo.GetTrainingCourses(domain);
            return Ok(courses);
        }

        [HttpGet("links/{domains}")]
        public async Task<IActionResult> GetLinks(string domains)
        {
            var validDomains = new[] { ".Net", "CRM", "iOS", "ETL", "RESRE", "ComplianceTrainings" };
            if (!validDomains.Contains(domains))
                return BadRequest("Invalid Domain");

            var links = await _repo.GetTrainingLinks(domains);
            return Ok(links);
        }

        [HttpGet("mandatory")]
        public async Task<IActionResult> GetMandatory()
        { 
            var trainings = await _repo.GetMandatoryTrainings();
            return Ok(trainings);
        }

    }
}
