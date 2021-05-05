using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IBloodDonorRepository
    {
        public Task<BloodDonorDTO> CreateBloodDonor(BloodDonorDTO bloodDonorDTO);
        public Task<BloodDonorDTO> UpdateBloodDonor(int Id, BloodDonorDTO bloodDonorDTO);
        public Task<BloodDonorDTO> GetBloodDonor(int Id);
        public Task<int> DeleteBloodDonor(int Id);
        public Task<IEnumerable<BloodDonorDTO>> GetAllBloodDonors();
    }
}
