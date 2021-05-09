using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IBloodRecipientRepository
    {
        public Task<BloodRecipientDTO> CreateBloodRecipient(BloodRecipientDTO bloodDonorDTO);
        public Task<BloodRecipientDTO> UpdateBloodRecipient(int Id, BloodRecipientDTO bloodDonorDTO);
        public Task<BloodRecipientDTO> GetBloodRecipient(int Id);
        public Task<int> DeleteBloodRecipient(int Id);
        public Task<IEnumerable<BloodRecipientDTO>> GetAllBloodRecipients();
    }
}
