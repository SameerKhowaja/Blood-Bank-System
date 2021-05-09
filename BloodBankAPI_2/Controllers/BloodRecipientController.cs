using Business.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace BloodBankAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodRecipientController : ControllerBase
    {
        private readonly IBloodRecipientRepository _bloodRecipientRepository;

        public BloodRecipientController(IBloodRecipientRepository bloodRecipientRepository)
        {
            _bloodRecipientRepository = bloodRecipientRepository;
        }

        /// <summary>
        /// Fetches List of All Blood Recipients from Database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBloodRecipients()
        {
            var bloodRecipients = await _bloodRecipientRepository.GetAllBloodRecipients();
            return Ok(bloodRecipients);
        }

        /// <summary>
        /// Fetches A Single Blood Recipient based on Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetBloodRecipient(int? Id)
        {
            if (Id == null)
            {
                RequestError error = new()
                {
                    Message = "BloodRecipient Id is required",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }

            var bloodRecipient = await _bloodRecipientRepository.GetBloodRecipient(Id.Value);

            if (bloodRecipient == null)
            {
                RequestError error = new()
                {
                    Message = $"No BloodRecipient found with Id: {Id}",
                    StatusCode = StatusCodes.Status404NotFound
                };
                return BadRequest(error);
            }

            /// BloodRecipient was found
            return Ok(bloodRecipient);
        }

        [HttpPost]
        public async Task<IActionResult> CreateBloodRecipient([FromBody] BloodRecipientDTO bloodRecipientDTO)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            var recipient = await _bloodRecipientRepository.CreateBloodRecipient(bloodRecipientDTO);

            if (recipient == null)
            {
                RequestError error = new()
                {
                    Message = "Could not create BloodRecipient",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }

            return Ok(recipient);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateBloodRecipient(int Id, [FromBody] BloodRecipientDTO updatedRecipient)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            var recipient = await _bloodRecipientRepository.UpdateBloodRecipient(Id, updatedRecipient);

            if (recipient == null)
            {
                RequestError error = new()
                {
                    Message = $"No BloodRecipient found with Id: {Id}",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }

            return Ok(recipient);
        }
    }
}
