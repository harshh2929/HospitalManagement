using AutoMapper;
using HospitalManagement.Models.Entity;

namespace HospitalManagement.MapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Admin, AdminDto>();
        }
    }
}
