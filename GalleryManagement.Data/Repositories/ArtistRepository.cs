using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;

namespace GalleryManagement.Data.Repositories
{
    public class ArtistRepository : Repository<Artist>, IArtistRepository
    {
        public ArtistRepository(GalleryDataContext context) : base(context)
        {
        }

        public List<Artist> GetByStatus(string status)
        {
            return _dbSet
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}