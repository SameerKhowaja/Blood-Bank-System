using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace BloodBankAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodGroupController : ControllerBase
    {
        private readonly IBloodGroupRepository _bloodGroupRepository;
        public BloodGroupController(IBloodGroupRepository bloodGroupRepository)
        {
            _bloodGroupRepository = bloodGroupRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetBloodGroups()
        {
            var bloodGroups = await _bloodGroupRepository.GetAllBloodGroups();
            return Ok(bloodGroups);
        }

        [HttpGet("{bloodType}")]
        public async Task<IActionResult> GetBloodGroup(string bloodType)
        {
            if (bloodType == null)
            {
                RequestError errorR = new()
                {
                    Message = "bloodType is required",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorR);
            }

            var bloodGroup = await _bloodGroupRepository
                .GetBloodGroup(bloodType);

            if (bloodGroup != null)
            {
                return Ok(bloodGroup);
            }
            RequestError error = new()
            {
                Message = $"No Records found for bloodType: {bloodType}",
                StatusCode = StatusCodes.Status404NotFound
            };
            return BadRequest(error);
        }
    }
}
