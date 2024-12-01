﻿using AutoMapper;
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

        CreateMap<IHobbyViewModel, IEditHobbyViewModel>()
            .ConstructUsingServiceLocator()
            .ForMember(dest => dest.EditDescription, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.EditName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        CreateMap<IEditHobbyViewModel, Hobby>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.EditName))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.EditDescription))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
    }
}
