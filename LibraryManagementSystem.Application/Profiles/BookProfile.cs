using AutoMapper;
using LibraryManagementSystem.Domain.Entities;
using static LibraryManagementSystem.Application.Dtos.BookDtos;

namespace LibraryManagementSystem.Application.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            //map createRequest to Book
            CreateMap<CreateRequest, Book>();

            //reverse map for it to work in both request and response order
            CreateMap<BookResponse, Book>().ReverseMap();

            CreateMap<UpdateRequest, Book>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

        }
    }

}
