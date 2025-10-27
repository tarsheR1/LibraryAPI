using AutoMapper;
using BusinessLogicLayer.Dto.Requests.Book;
using BusinessLogicLayer.Dto.Responses.Book;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookEntity, BookResponseDto>()
                .ForMember(dest => dest.BookAge, opt => opt.MapFrom(src =>
                    DateTime.Now.Year - src.PublishedYear))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src =>
                    src.Author != null ? src.Author.Name : string.Empty));

            CreateMap<CreateBookDto, BookEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Author, opt => opt.Ignore());

            CreateMap<UpdateBookDto, BookEntity>()
                .ForMember(dest => dest.Author, opt => opt.Ignore());
        }
    }
}
