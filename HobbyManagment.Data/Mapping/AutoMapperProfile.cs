using AutoMapper;
using HobbyManagment.Data;
using HobbyManagment.Data.Database.Models;

namespace HobbyManagement.Mapping;

class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Hobby, Hobby>();
        CreateMap<HobbyEntity, Hobby>().ReverseMap();
    }
}
