using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IArtworkRepository
    {
        List<Artwork> GetAll();
        List<Artwork> GetByStatus(string status);
        List<Artwork> GetByArtistId(int artistId);
        Artwork? GetById(int id);
        Artwork Add(Artwork artwork);
        Artwork Update(Artwork artwork);
        void Delete(int id);
        int GetNextId();
    }
}
