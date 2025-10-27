using AutoMapper;
using BusinessLogicLayer.Dto.Requests.Author;
using BusinessLogicLayer.Dto.Responses.Author;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorEntity, AuthorResponseDto>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src =>
                    DateOnly.FromDateTime(DateTime.Now).Year - src.DateOfBirth.Year))
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src =>
                    src.Books != null ? src.Books.Count : 0));

            CreateMap<CreateAuthorDto, AuthorEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.Books, opt => opt.Ignore());


            CreateMap<UpdateAuthorDto, AuthorEntity>()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }

}
