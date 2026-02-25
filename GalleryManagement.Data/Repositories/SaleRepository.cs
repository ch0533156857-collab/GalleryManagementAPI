using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace GalleryManagement.Data.Repositories
{
    public class SaleRepository : Repository<Sale>, ISaleRepository
    {
        public SaleRepository(GalleryDataContext context) : base(context)
        {
        }

        public async Task<List<Sale>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToListAsync();
        }

        public async Task<List<Sale>> GetByArtworkIdAsync(int artworkId)
        {
            return await _dbSet
                .Where(s => s.ArtworkId == artworkId)
                .ToListAsync();
        }
    }
}