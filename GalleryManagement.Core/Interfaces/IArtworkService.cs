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
        List<Artwork> GetAllArtworks(string? status = null);
        List<Artwork> GetArtworksByArtist(int artistId);
        Artwork? GetArtworkById(int id);
        Artwork CreateArtwork(Artwork artwork);
        Artwork UpdateArtwork(int id, Artwork updatedArtwork);
        Artwork UpdateArtworkStatus(int id, string status);
        void DeleteArtwork(int id);
    }
}
