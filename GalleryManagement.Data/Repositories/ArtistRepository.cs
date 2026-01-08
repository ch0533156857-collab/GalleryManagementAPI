using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;

namespace GalleryManagement.Data.Repositories
{
    public class ArtistRepository : IArtistRepository
    {
        private readonly GalleryDataContext _context;

        public ArtistRepository(GalleryDataContext context)
        {
            _context = context;
        }

        public List<Artist> GetAll()
        {
            return _context.Artists.ToList();
        }

        public List<Artist> GetByStatus(string status)
        {
            return _context.Artists
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public Artist? GetById(int id)
        {
            return _context.Artists.Find(id);
        }

        public Artist Add(Artist artist)
        {
            artist.CreatedAt = DateTime.Now;
            artist.Status = "active";
            _context.Artists.Add(artist);
            _context.SaveChanges();
            return artist;
        }

        public Artist Update(Artist artist)
        {
            var existingArtist = GetById(artist.Id);
            if (existingArtist != null)
            {
                existingArtist.Name = artist.Name;
                existingArtist.Biography = artist.Biography;
                existingArtist.Nationality = artist.Nationality;
                existingArtist.BirthDate = artist.BirthDate;
                existingArtist.Style = artist.Style;
                existingArtist.Status = artist.Status;
                _context.SaveChanges();
            }
            return existingArtist!;
        }

        public int GetNextId()
        {
            // EF מנהל את ה-ID אוטומטית
            return _context.Artists.Any() ? _context.Artists.Max(a => a.Id) + 1 : 1;
        }
    }
}