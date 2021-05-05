using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace BloodBankAPI_2.Controllers
{
    [Route("api/[controller]")]
    public class BloodDonorController : Controller
    {
        private readonly IBloodDonorRepository _bloodDonorRepository;

        public BloodDonorController(IBloodDonorRepository bloodDonorRepository)
        {
            _bloodDonorRepository = bloodDonorRepository;
        }

        /// <summary>
        /// Fetchs List of All Blood Donors from Database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBloodDonors()
        {
            var bloodDonors = await _bloodDonorRepository.GetAllBloodDonors();
            return Ok(bloodDonors);
        }

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
    }
}
