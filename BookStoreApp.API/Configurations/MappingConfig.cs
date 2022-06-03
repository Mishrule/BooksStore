using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;
using BookStoreApp.API.Models.User;

namespace BookStoreApp.API.Configurations
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
          CreateMap<AuthorCreateDto, Author>().ReverseMap();
          CreateMap<AuthorReadOnlyDto, Author>().ReverseMap();
          CreateMap<AuthorUpdateDto, Author>().ReverseMap();
          CreateMap<AuthorDetailsDto, Author>().ReverseMap();
          
          CreateMap<BookCreateDto, Book>().ReverseMap();
          CreateMap<BookUpdateDto, Book>().ReverseMap();
          CreateMap<Book, BookReadOnlyDto>()
            .ForMember(q => q.AuthorName, opt => opt.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();

          CreateMap<Book, BookDetailsDto>()
              .ForMember(q => q.AuthorName, opt => opt.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
              .ReverseMap();


      CreateMap<ApiUser, UserDto>().ReverseMap();
    }
    }
}
