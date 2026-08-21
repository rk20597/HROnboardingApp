using Microsoft.AspNetCore.Mvc;
using HROnboarding.API.Repositories;
using Microsoft.AspNetCore.Authorization;


namespace HROnboarding.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CandidatesController : ControllerBase
    {
        private readonly ExcelRepository _repo;

        public CandidatesController(ExcelRepository repo) 
        { 
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            var candidates = await _repo.GetAllCandidates();
            return Ok(candidates);
        }

        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        { 
            var candidates = await _repo.GetActiveCandidates();
            return Ok(candidates);
        }

        [HttpGet("inactive")]
        public async Task<IActionResult> GetInactive()
        { 
            var candidates = _repo.GetInactiveCandidates();
            return Ok(candidates);
        }

        [HttpGet("test")]
        [AllowAnonymous]
        public IActionResult Test()
        {
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "..", "..", "Data", "HRData.xlsx");

            if (!System.IO.File.Exists(path))
                return Ok("File NOT found at: " + path);

            using var package = new OfficeOpenXml
                .ExcelPackage(new System.IO.FileInfo(path));

            var sheetCount = package.Workbook
                .Worksheets.Count;

            var sheetNames = "";
            foreach (var ws in package.Workbook.Worksheets)
            {
                sheetNames += ws.Name + " | ";
            }

            return Ok(new
            {
                path = path,
                sheetCount = sheetCount,
                sheets = sheetNames
            });
        }

        


    }
}
