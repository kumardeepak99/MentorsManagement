using MentorsManagement.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace MentorsManagement.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MentorsController : ControllerBase
    {
        private readonly IMentorService _mentorsService;
        public MentorsController(IMentorService mentorsService)
        {
            _mentorsService = mentorsService;
        }

        [HttpGet("GetAllAsync")]
        public async Task<IActionResult> GetAllMentorsAsync()
        {
            try
            {
                var mentors = await _mentorsService.GetAllMentors();
                if (mentors.Any())
                {
                    return Ok(mentors);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}