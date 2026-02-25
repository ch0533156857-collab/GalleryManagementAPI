using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Artwork>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<List<Artwork>> GetByArtistIdAsync(int artistId)
        {
            return await _dbSet
                .Where(a => a.ArtistId == artistId)
                .ToListAsync();
        }
    }
}
