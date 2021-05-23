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
    public class ReceptionRecordRepository : IReceptionRecordRepository
    {
        private readonly ApplicationDbContext _db;
        public ReceptionRecordRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<ReceptionRecord> CreateReceptionRecord(ReceptionRecord receptionRecord)
        {
            receptionRecord.ReceivedAt = DateTime.UtcNow;
            var addedReceptionRecord = _db.ReceptionRecords.Update(receptionRecord).Entity;

            await _db.SaveChangesAsync();
            return addedReceptionRecord;
        }

        public async Task<IEnumerable<ReceptionRecord>> GetReceptionRecords()
        {
            return await _db.ReceptionRecords.Include(record => record.ReceivedBy).ToListAsync();
        }

        public async Task<IEnumerable<ReceptionRecord>> GetReceptionRecordsByRecipientId(int recipientId)
        {
            var receptionRecord = await _db.ReceptionRecords
                .Where(record => record.ReceivedBy.Id == recipientId).ToListAsync();

            return receptionRecord;
        }
    }
}
