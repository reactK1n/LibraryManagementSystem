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
              .ForMember(dest => dest.Title, opt =>
                  opt.Condition(src => src.Title != null))
              .ForMember(dest => dest.Author, opt =>
                  opt.Condition(src => src.Author != null))
              .ForMember(dest => dest.ISBN, opt =>
                  opt.Condition(src => src.ISBN != null))
              .ForMember(dest => dest.PublishedDate, opt =>
                  opt.Condition(src => src.PublishedDate != null));


        }
    }

}
