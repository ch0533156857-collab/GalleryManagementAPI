using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IArtworkRepository : IRepository<Artwork>
    {
        List<Artwork> GetByStatus(string status);
        List<Artwork> GetByArtistId(int artistId);
    }
}
