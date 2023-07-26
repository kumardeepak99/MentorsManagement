using MentorsManagement.API.Models;
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

        [HttpGet("GetAllMentorsAsync")]
        public async Task<IActionResult> GetAllMentorsAsync()
        {
            try
            {
                var mentors = await _mentorsService.GetAllMentorsAsync();
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

        [HttpGet("GetMentorByIdAsync/{id}")]
        public async Task<IActionResult> GetMentorByIdAsync(string id)
        {
            try
            {
                var mentor = await _mentorsService.GetMentorByIdAsync(id);
                if (mentor != null)
                {
                    return Ok(mentor);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("CreateMentorAsync")]
        public async Task<IActionResult> CreateMentorAsync([FromBody] Mentor mentor)
        {
            try
            {
                if (!string.IsNullOrEmpty(mentor.Id))
                {
                    throw new ArgumentNullException(nameof(mentor.Id), "The Mentor ID should not be provided when creating a new Mentor.");
                }
                var createdMentor = await _mentorsService.CreateMentorAsync(mentor);
                return Ok(createdMentor);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("UpdateMentorAsync")]
        public async Task<IActionResult> UpdateMentorAsync([FromBody] Mentor mentor)
        {
            try
            {
                var updatedMentor = await _mentorsService.UpdateMentorAsync(mentor);
                if (updatedMentor != null)
                {
                    return Ok(updatedMentor);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("DeleteMentorAsync/{id}")]
        public async Task<IActionResult> DeleteMentorAsync(string id)
        {
            try
            {
                _mentorsService.DeleteMentorAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

    }
}