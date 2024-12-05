using AutoMapper;
using HobbyManagment.Data;

namespace HobbyManagement.Mapping;

/// <summary>
/// Auto Mapper profile.
/// </summary>
class AutoMapperProfile : Profile
{
    /// <summary>
    /// Constructor that creates mappings.
    /// </summary>
    public AutoMapperProfile()
    {
        CreateMap<Hobby, Hobby>();
    }
}
