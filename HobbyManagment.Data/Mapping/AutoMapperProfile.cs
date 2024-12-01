using AutoMapper;
using HobbyManagment.Data;

namespace HobbyManagement.Mapping;

class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Hobby, Hobby>();
    }
}
