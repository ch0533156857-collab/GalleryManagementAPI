using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;

namespace GalleryManagement.Data.Repositories
{
    public class ExhibitionRepository : Repository<Exhibition>, IExhibitionRepository
    {
        public ExhibitionRepository(GalleryDataContext context) : base(context)
        {
        }

        public List<Exhibition> GetActive()
        {
            var today = DateTime.Now;
            return _dbSet
                .Where(e => e.StartDate <= today && e.EndDate >= today)
                .ToList();
        }
    }
}