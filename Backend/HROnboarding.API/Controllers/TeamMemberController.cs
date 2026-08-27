using Microsoft.AspNetCore.Mvc;
using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamMemberController : ControllerBase
    {
        private readonly TeamTrackerRepository _repo;
        public TeamMemberController(TeamTrackerRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("active")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetActive()
        {
            var members = await _repo.GetActiveMembers();
            
            return Ok(members);
        }

        [HttpGet("inactive")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetInactive()
        {
            var members = await _repo.GetInactiveMembers();

            return Ok(members);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var members = await _repo.GetAllMembers();

            return Ok(members);
        }

        [HttpGet("debug")]
        [AllowAnonymous]
        public async Task<IActionResult> Debug()
        {
            try
            {
                var all = await _repo.GetAllMembers();
                return Ok(new
                {
                    totalCount = all.Count,
                    activeCount = all.Count(m =>
                        m.Status?.ToLower() == "active"),
                    firstMember = all.FirstOrDefault()
                });
            }
            catch (Exception ex)
            {
                return Ok(new { error = ex.Message });
            }
        }
    }
}
