using DataAccess.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IReceptionRecordRepository
    {
        public Task<ReceptionRecord> CreateReceptionRecord(ReceptionRecord receptionRecord);
        public Task<IEnumerable<ReceptionRecord>> GetReceptionRecords();
        public Task<IEnumerable<ReceptionRecord>> GetReceptionRecordsByRecipientId(int recipientId);
    }
}
