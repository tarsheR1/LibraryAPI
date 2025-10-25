using AutoMapper;
using BusinessLogicLayer.Models;
using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Mapping
{
    public class AuthorProfile : Profile
    {
        public AuthorProfile()
        {
            CreateMap<AuthorEntity, Author>();
            CreateMap<Author, AuthorEntity>()
                .ForMember(dest => dest.Books, opt => opt.Ignore());
        }
    }

}
