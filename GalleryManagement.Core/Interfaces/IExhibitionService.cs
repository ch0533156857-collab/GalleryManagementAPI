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
        Task<List<Exhibition>> GetAllExhibitionsAsync();
        Task<List<Exhibition>> GetActiveExhibitionsAsync();
        Task<Exhibition?> GetExhibitionByIdAsync(int id);
        Task<Exhibition> CreateExhibitionAsync(Exhibition exhibition);
        Task<Exhibition> UpdateExhibitionAsync(int id, Exhibition updatedExhibition);
        Task<Exhibition> AddArtworkToExhibitionAsync(int exhibitionId, int artworkId);
        Task<Exhibition> RemoveArtworkFromExhibitionAsync(int exhibitionId, int artworkId);
        Task DeleteExhibitionAsync(int id);
    }
}
