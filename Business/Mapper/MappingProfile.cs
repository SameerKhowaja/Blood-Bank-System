using AutoMapper;
using DataAccess.Data;
using Models;

namespace Business.Mapper
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<BloodDonorDTO, BloodDonor>();
            CreateMap<BloodDonor, BloodDonorDTO>();
            CreateMap<BloodRecipient, BloodRecipientDTO>();
            CreateMap<BloodRecipientDTO, BloodRecipient>();
        }
    }
}
