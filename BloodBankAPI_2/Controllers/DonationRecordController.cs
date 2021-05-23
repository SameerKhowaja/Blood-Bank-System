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
    public class DonationRecordController : ControllerBase
    {
        private readonly IDonationRecordRepository _donationRecordRepository;
        private readonly IBloodGroupRepository _bloodGroupRepository;

        public DonationRecordController(IDonationRecordRepository donationRecordRepository,
            IBloodGroupRepository bloodGroupRepository)
        {
            _donationRecordRepository = donationRecordRepository;
            _bloodGroupRepository = bloodGroupRepository;
            
        }

        [HttpGet]
        public async Task<IActionResult> GetDonationRecords()
        {
            var donationRecords = await _donationRecordRepository.GetDonationRecords();
            return Ok(donationRecords);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetDonationRecordsByDonorId(int? Id)
        {
            if(Id == null)
            {
                RequestError errorR = new()
                {
                    Message = "BloodDonor Id is required",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(errorR);
            }

            var donationRecords = await _donationRecordRepository
                .GetDonationRecordsByDonorId(Id.Value);

            if (donationRecords.Any())
            {
                return Ok(donationRecords);
            }
            RequestError error = new() {
                Message = $"No DonationRecords found for Id: {Id}",
                StatusCode = StatusCodes.Status404NotFound
            };
            return BadRequest(error);
        }

        [HttpPost]
        public async Task<IActionResult> CreateDonationRecord([FromBody] DonationRecord donationRecord)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);

            var record = await _donationRecordRepository.CreateDonationRecord(donationRecord);

            if(record == null)
            {
                RequestError error = new()
                {
                    Message = "Could not create DonationRecord",
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return BadRequest(error);
            }
            /// Update BloodGroup
            BloodGroup bloodGroup = new() {BloodType=record.DonatedBy.BloodType, Units=record.Units };
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
