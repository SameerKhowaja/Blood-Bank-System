using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloodBankAPI_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceptionRecordController : ControllerBase
    {
        private readonly IReceptionRecordRepository _receptionRecordRepository;
        private readonly IBloodGroupRepository _bloodGroupRepository;

        public ReceptionRecordController(IReceptionRecordRepository receptionRecordRepository, IBloodGroupRepository bloodGroupRepository)
        {
            _receptionRecordRepository = receptionRecordRepository;
            _bloodGroupRepository = bloodGroupRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetReceptionRecords()
        {
            var receptionRecords = await _receptionRecordRepository.GetReceptionRecords();
            return Ok(receptionRecords);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetReceptionRecordsByRecipientId(int? Id)
        {
            if (Id == null)
            {
                RequestError errorR = new()
                {
                    Message = "BloodDonor Id is required",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorR);
            }

            var receptionRecords = await _receptionRecordRepository
                .GetReceptionRecordsByRecipientId(Id.Value);

            if (receptionRecords.Any())
            {
                return Ok(receptionRecords);
            }
            RequestError error = new()
            {
                Message = $"No ReceptionRecords found for Id: {Id}",
                StatusCode = StatusCodes.Status404NotFound
            };
            return BadRequest(error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReceptionRecord([FromBody] ReceptionRecord receptionRecord)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            var record = await _receptionRecordRepository.CreateReceptionRecord(receptionRecord);
            if (record == null)
            {
                RequestError error = new()
                {
                    Message = "Could not create ReceptionRecord",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }
            /// Update BloodGroup
            BloodGroup bloodGroup = new() { BloodType = record.ReceivedBy.BloodType, Units = (-1 * record.Units) };
            var group = await _bloodGroupRepository.UpdateBloodGroup(bloodGroup);

            if (group == null)
            {
                RequestError error = new()
                {
                    Message = "Could not update BloodGroup",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }

            return Ok(record);
        }
    }
}
