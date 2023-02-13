using AutoMapper;
using BookAPI.DTO.Entities;
using BookAPI.Models.Entities;

namespace FatecLibrary.BookAPI.DTO.Mappings;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<Publishing, PublishingDTO>().ReverseMap();
        CreateMap<BookDTO, BookDTO>();

        CreateMap<Book, BookDTO>().ForMember(
            book => book.PublishingName,
            options => options.MapFrom(
                src => src.Publishing.Name
            )
        );
    }
}