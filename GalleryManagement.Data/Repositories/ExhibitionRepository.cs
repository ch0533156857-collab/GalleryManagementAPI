using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using Microsoft.EntityFrameworkCore;

namespace GalleryManagement.Data.Repositories
{
    public class ExhibitionRepository : Repository<Exhibition>, IExhibitionRepository
    {
        public ExhibitionRepository(GalleryDataContext context) : base(context)
        {
        }

        public async Task<List<Exhibition>> GetActiveAsync()
        {
            var today = DateTime.Now;
            return await _dbSet
                .Where(e => e.StartDate <= today && e.EndDate >= today)
                .ToListAsync();
        }
    }
}