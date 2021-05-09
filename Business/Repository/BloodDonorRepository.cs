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
    public class BloodDonorRepository : IBloodDonorRepository
    {
        private readonly ApplicationDbContext _db;

        private readonly IMapper _mapper;

        public BloodDonorRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        public async Task<BloodDonorDTO> CreateBloodDonor(BloodDonorDTO bloodDonorDTO)
        {
            var bloodDonor = _mapper.Map<BloodDonorDTO, BloodDonor>(bloodDonorDTO);
            bloodDonor.CreatedAt = DateTime.UtcNow;
            var addedBloodDonor = await _db.BloodDonors.AddAsync(bloodDonor);

            /// Save changes to DB
            await _db.SaveChangesAsync();
            return _mapper.Map<BloodDonor, BloodDonorDTO>(addedBloodDonor.Entity);
        }

        public async Task<int> DeleteBloodDonor(int Id)
        {
            var bloodDonor = await _db.BloodDonors.FindAsync(Id);
            if (bloodDonor == null) return 0;

            /// Save to DB
            _db.BloodDonors.Remove(bloodDonor);
            return await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<BloodDonorDTO>> GetAllBloodDonors()
        {
            try
            {
                var bloodDonorDTOs = _mapper
                    .Map<IEnumerable<BloodDonor>, IEnumerable<BloodDonorDTO>>(_db.BloodDonors);

                return bloodDonorDTOs;
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<BloodDonorDTO> GetBloodDonor(int Id)
        {
            try
            {
                var bloodDonor = await _db.BloodDonors.FirstOrDefaultAsync(donor => donor.Id == Id);
                return _mapper.Map<BloodDonor, BloodDonorDTO>(bloodDonor);
            } catch(Exception ex)
            {
                return null;
            }
        }

        public async Task<BloodDonorDTO> UpdateBloodDonor(int Id, BloodDonorDTO bloodDonorDTO)
        {
            try
            {
                var bloodDonor = await _db.BloodDonors.FindAsync(Id);
                var donor = _mapper.Map<BloodDonorDTO, BloodDonor>(bloodDonorDTO, bloodDonor);

                var updatedDonor = _db.BloodDonors.Update(donor).Entity;

                /// Save changes to DB
                await _db.SaveChangesAsync();
                return _mapper.Map<BloodDonor, BloodDonorDTO>(updatedDonor);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
