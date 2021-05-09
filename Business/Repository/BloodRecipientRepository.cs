using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class BloodRecipientRepository : IBloodRecipientRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        public BloodRecipientRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BloodRecipientDTO> CreateBloodRecipient(BloodRecipientDTO bloodDonorDTO)
        {
            var bloodRecipient = _mapper.Map<BloodRecipientDTO, BloodRecipient>(bloodDonorDTO);
            bloodRecipient.CreatedAt = DateTime.UtcNow;
            var addedBloodRecipient = await _db.BloodRecipients.AddAsync(bloodRecipient);

            await _db.SaveChangesAsync();
            return _mapper.Map<BloodRecipient, BloodRecipientDTO>(addedBloodRecipient.Entity);
        }

        public async Task<int> DeleteBloodRecipient(int Id)
        {
            var bloodRecipient = await _db.BloodRecipients.FindAsync(Id);
            if (bloodRecipient == null) return 0;

            /// Save to DB
            _db.BloodRecipients.Remove(bloodRecipient);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloodRecipientDTO>> GetAllBloodRecipients()
        {
            var bloodRecipients = _mapper.Map<IEnumerable<BloodRecipient>, IEnumerable<BloodRecipientDTO>>(_db.BloodRecipients);
            return bloodRecipients;
        }

        public async Task<BloodRecipientDTO> GetBloodRecipient(int Id)
        {
            var bloodRecipient = await _db.BloodRecipients.FirstOrDefaultAsync(recipient => recipient.Id == Id);
            return _mapper.Map<BloodRecipient, BloodRecipientDTO>(bloodRecipient);

        }

        public async Task<BloodRecipientDTO> UpdateBloodRecipient(int Id, BloodRecipientDTO bloodRecipientDTO)
        {
            var bloodRecipient = await _db.BloodRecipients.FindAsync(Id);
            var recipient = _mapper.Map<BloodRecipientDTO, BloodRecipient>(bloodRecipientDTO, bloodRecipient);

            var updatedRecipient =  _db.BloodRecipients.Update(recipient).Entity;

            await _db.SaveChangesAsync();
            return _mapper.Map<BloodRecipient, BloodRecipientDTO>(updatedRecipient);
        }
    }
}
