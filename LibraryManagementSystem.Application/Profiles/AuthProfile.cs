using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.Data;
using static LibraryManagementSystem.Application.Dtos.AuthDtos;

namespace LibraryManagementSystem.Application.Profiles
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
             .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
             .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => true));


            CreateMap<ApplicationUser, LoginResponse>();
        }
    }
}
