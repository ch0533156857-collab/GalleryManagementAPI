using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;

namespace GalleryManagement.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Artist, ArtistDTO>().ReverseMap();
            CreateMap<Artwork, ArtworkDTO>().ReverseMap();
            CreateMap<Exhibition, ExhibitionDTO>().ReverseMap();
            CreateMap<Sale, SaleDTO>().ReverseMap();
        }   
    }
}