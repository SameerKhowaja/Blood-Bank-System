using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IDonationRecordRepository
    {
        public Task<DonationRecord> CreateDonationRecord(DonationRecord donationRecord);
        public Task<IEnumerable<DonationRecord>> GetDonationRecords();
        public Task<IEnumerable<DonationRecord>> GetDonationRecordsByDonorId(int donorId);
    }
}
