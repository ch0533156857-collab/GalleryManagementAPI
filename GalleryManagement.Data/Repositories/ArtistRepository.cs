using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace GalleryManagement.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(GalleryDataContext context) : base(context)
        {
        }

        public async Task<List<Artist>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }
    }
}