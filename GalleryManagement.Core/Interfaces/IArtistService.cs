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
        List<Artist> GetAllArtists(string? status = null);
        Artist? GetArtistById(int id);
        Artist CreateArtist(Artist artist);
        Artist UpdateArtist(int id, Artist updatedArtist);
        Artist UpdateArtistStatus(int id, string status);
        List<Artwork> GetArtistArtworks(int id);
    }
}
