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
    public class DonationRecordRepository : IDonationRecordRepository
    {
        private readonly ApplicationDbContext _db;

        public DonationRecordRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<DonationRecord> CreateDonationRecord(DonationRecord donationRecord)
        {
            donationRecord.DonatedAt = DateTime.UtcNow;
            var addedDonationRecord = _db.DonationRecords.Update(donationRecord).Entity;

            await _db.SaveChangesAsync();
            return addedDonationRecord;
        }

        public async Task<IEnumerable<DonationRecord>> GetDonationRecords()
        {
            return await _db.DonationRecords.Include(record => record.DonatedBy).ToListAsync();
        }

        public async Task<IEnumerable<DonationRecord>> GetDonationRecordsByDonorId(int donorId)
        {
            var donationRecords = await _db.DonationRecords
                .Where(record => record.DonatedBy.Id == donorId).ToListAsync();

            return donationRecords;
        }
    }
}
