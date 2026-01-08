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

        public List<Sale> GetByStatus(string status)
        {
            return _dbSet
                .Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Sale> GetByArtworkId(int artworkId)
        {
            return _dbSet
                .Where(s => s.ArtworkId == artworkId)
                .ToList();
        }
    }
}