using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;

namespace GalleryManagement.Data.Repositories
{
    public class ExhibitionRepository : IExhibitionRepository
    {
        private readonly GalleryDataContext _context;

        public ExhibitionRepository(GalleryDataContext context)
        {
            _context = context;
        }

        public List<Exhibition> GetAll()
        {
            return _context.Exhibitions.ToList();
        }

        public List<Exhibition> GetActive()
        {
            var today = DateTime.Now;
            return _context.Exhibitions
                .Where(e => e.StartDate <= today && e.EndDate >= today)
                .ToList();
        }

        public Exhibition? GetById(int id)
        {
            return _context.Exhibitions.FirstOrDefault(e => e.Id == id);
        }

        public Exhibition Add(Exhibition exhibition)
        {
            _context.Exhibitions.Add(exhibition);
            _context.SaveChanges();
            return exhibition;
        }

        public Exhibition Update(Exhibition exhibition)
        {
            var existingExhibition = GetById(exhibition.Id);
            if (existingExhibition != null)
            {
                existingExhibition.Name = exhibition.Name;
                existingExhibition.Description = exhibition.Description;
                existingExhibition.StartDate = exhibition.StartDate;
                existingExhibition.EndDate = exhibition.EndDate;
                existingExhibition.Location = exhibition.Location;
                existingExhibition.CuratorName = exhibition.CuratorName;
                existingExhibition.ArtworkIds = exhibition.ArtworkIds;
                _context.SaveChanges();
            }
            return existingExhibition!;
        }

        public void Delete(int id)
        {
            var exhibition = GetById(id);
            if (exhibition != null)
            {
                _context.Exhibitions.Remove(exhibition);
                _context.SaveChanges();
            }
        }

        public int GetNextId()
        {
            // EF מנהל את ה-ID אוטומטית
            return _context.Exhibitions.Any() ? _context.Exhibitions.Max(e => e.Id) + 1 : 1;
        }
    }
}