using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace BloodBankAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodDonorController : ControllerBase
    {
        private readonly IBloodDonorRepository _bloodDonorRepository;

        public BloodDonorController(IBloodDonorRepository bloodDonorRepository)
        {
            _bloodDonorRepository = bloodDonorRepository;
        }

        /// <summary>
        /// Fetches List of All Blood Donors from Database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBloodDonors()
        {
            var bloodDonors = await _bloodDonorRepository.GetAllBloodDonors();
            return Ok(bloodDonors);
        }

        /// <summary>
        /// Fetches A Sngle Blood Donor based on Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBloodDonor(int? Id)
        {
            if (Id == null)
            {
                RequestError error = new(){
                    Message = "BloodDonor Id is required",
                    StatusCode = StatusCodes.Status400BadRequest
                    };
                return BadRequest(error);
            }

            var bloodDonor = await _bloodDonorRepository.GetBloodDonor(Id.Value);

            if(bloodDonor == null)
            {
                RequestError error = new()
                {
                    Message = $"No BloodDonor found with Id: {Id}",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return BadRequest(error);
            }

            /// BloodDonor was found
            return Ok(bloodDonor);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBloodDonor([FromBody] BloodDonorDTO bloodDonorDTO)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            var donor = await _bloodDonorRepository.CreateBloodDonor(bloodDonorDTO);

            if(donor == null)
            {
                RequestError error = new()
                {
                    Message = "Could not create BloodDonor",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }

            return Ok(donor);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBloodDonor(int Id, [FromBody] BloodDonorDTO updatedDonor)
        {
            if(ModelState.IsValid == false) return BadRequest(ModelState);

            var donor = await _bloodDonorRepository.UpdateBloodDonor(Id, updatedDonor);

            if(donor == null)
            {
                RequestError error = new()
                {
                    Message = $"No BloodDonor found with Id: {Id}",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }

            return Ok(donor);
        }
    }
}
