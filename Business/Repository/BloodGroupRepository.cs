
using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class BloodGroupRepository : IBloodGroupRepository
    {
        private readonly ApplicationDbContext _db;

        public BloodGroupRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<BloodGroup>> GetAllBloodGroups()
        {
            return _db.BloodGroups.ToList();
        }

        public async Task<BloodGroup> GetBloodGroup(string bloodType)
        {
            try
            {
                return await _db.BloodGroups.FirstOrDefaultAsync(group => group.BloodType == bloodType);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<BloodGroup> UpdateBloodGroup(BloodGroup bloodGroup)
        {
            try
            {
                var group = await _db.BloodGroups.FirstOrDefaultAsync(group => group.BloodType == bloodGroup.BloodType);
                group.Units += bloodGroup.Units;

                var updatedGroup = _db.BloodGroups.Update(group).Entity;

                /// Save changes to DB
                await _db.SaveChangesAsync();
                return updatedGroup;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
