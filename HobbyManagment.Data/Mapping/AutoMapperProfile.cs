using AutoMapper;
using HobbyManagment.Data;
using HobbyManagment.Data.Models;

namespace HobbyManagement.Mapping;

class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Hobby, Hobby>();
        CreateMap<HobbyEntity, Hobby>().ReverseMap();
    }
}
