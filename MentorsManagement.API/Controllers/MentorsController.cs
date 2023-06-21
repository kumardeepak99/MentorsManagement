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

        [HttpGet("GetMentorByIdAsync/{id}")]
        public async Task<IActionResult> GetMentorByIdAsync(int id)
        {
            try
            {
                var mentor = await _mentorsService.GetMentorById(id);
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
                if (mentor.MentorId<=0)
                {
                    throw new ArgumentException();
                }
                var createdMentor = await _mentorsService.CreateMentor(mentor);
                if (createdMentor == null)
                {
                    return Conflict();
                }
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
                var updatedMentor = await _mentorsService.UpdateMentor(mentor);
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
        public async Task<IActionResult> DeleteMentorAsync(int id)
        {
            try
            {
                var deleted = await _mentorsService.DeleteMentor(id);
                if (deleted)
                {
                    return NoContent();
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