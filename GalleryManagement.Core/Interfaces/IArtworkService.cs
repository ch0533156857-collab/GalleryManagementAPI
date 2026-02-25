using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IArtworkService
    {
        Task<List<Artwork>> GetAllArtworksAsync(string? status = null);
        Task<List<Artwork>> GetArtworksByArtistAsync(int artistId);
        Task<Artwork?> GetArtworkByIdAsync(int id);
        Task<Artwork> CreateArtworkAsync(Artwork artwork);
        Task<Artwork> UpdateArtworkAsync(int id, Artwork updatedArtwork);
        Task<Artwork> UpdateArtworkStatusAsync(int id, string status);
        Task DeleteArtworkAsync(int id);
    }
}
