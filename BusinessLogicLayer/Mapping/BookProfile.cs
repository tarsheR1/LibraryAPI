using AutoMapper;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookEntity, Book>();
            CreateMap<Book, BookEntity>()
                .ForMember(dest => dest.Author, opt => opt.Ignore());
        }
    }

}
