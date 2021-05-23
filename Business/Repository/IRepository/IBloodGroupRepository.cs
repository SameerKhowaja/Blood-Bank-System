using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IBloodGroupRepository
    {
        public Task<BloodGroup> UpdateBloodGroup(BloodGroup bloodGroup);
        public Task<BloodGroup> GetBloodGroup(string bloodType);
        public Task<List<BloodGroup>> GetAllBloodGroups();
    }
}
