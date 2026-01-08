using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Data.Repositories
{
    public class ArtworkRepository : Repository<Artwork>, IArtworkRepository
    {
        public ArtworkRepository(GalleryDataContext context) : base(context)
        {
        }

        public List<Artwork> GetByStatus(string status)
        {
            return _dbSet
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Artwork> GetByArtistId(int artistId)
        {
            return _dbSet
                .Where(a => a.ArtistId == artistId)
                .ToList();
        }
    }
}
