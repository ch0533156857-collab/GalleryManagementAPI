using GalleryManagement.Core.Entities;
using restful_code.Models.Artist;
using restful_code.Models.Artwork;
using restful_code.Models.Exhibition;
using restful_code.Models.Sale;

namespace restful_code.Mapping
{
    public class ApiMappingProfile : AutoMapper.Profile
    {
        public ApiMappingProfile()
        {
            CreateMap<CreateArtistModel, Artist>();
            CreateMap<UpdateArtistModel, Artist>();

            CreateMap<CreateArtworkModel, Artwork>();
            CreateMap<UpdateArtworkModel, Artwork>();

            CreateMap<CreateExhibitionModel, Exhibition>();
            CreateMap<UpdateExhibitionModel, Exhibition>();

            CreateMap<CreateSaleModel, Sale>();
            CreateMap<UpdateSaleModel, Sale>();
        }
    }
}
