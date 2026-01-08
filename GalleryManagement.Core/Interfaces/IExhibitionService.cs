using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IExhibitionService
    {
        List<Exhibition> GetAllExhibitions();
        List<Exhibition> GetActiveExhibitions();
        Exhibition? GetExhibitionById(int id);
        Exhibition CreateExhibition(Exhibition exhibition);
        Exhibition UpdateExhibition(int id, Exhibition updatedExhibition);
        Exhibition AddArtworkToExhibition(int exhibitionId, int artworkId);
        Exhibition RemoveArtworkFromExhibition(int exhibitionId, int artworkId);
        void DeleteExhibition(int id);
    }
}
