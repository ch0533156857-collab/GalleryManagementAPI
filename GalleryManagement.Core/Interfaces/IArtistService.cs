using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IArtistService
    {
        Task<List<Artist>> GetAllArtistsAsync(string? status = null);
        Task<Artist?> GetArtistByIdAsync(int id);
        Task<Artist> CreateArtistAsync(Artist artist);
        Task<Artist> UpdateArtistAsync(int id, Artist updatedArtist);
        Task<Artist> UpdateArtistStatusAsync(int id, string status);
        Task<List<Artwork>> GetArtistArtworksAsync(int id);
    }
}
