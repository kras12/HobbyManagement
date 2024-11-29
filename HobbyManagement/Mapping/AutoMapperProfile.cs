using AutoMapper;
using HobbyManagement.Viewmodels;
using HobbyManagment.Data;

namespace HobbyManagement.Mapping;

class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Hobby, IHobbyViewModel>()
        .ConstructUsingServiceLocator()
        .AfterMap((src, dest) =>
        {
            dest.SetWrappedHobby(src);
        });
    }
}
