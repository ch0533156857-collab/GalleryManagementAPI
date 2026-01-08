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
    public class ArtworkRepository : IArtworkRepository
    {
        private readonly GalleryDataContext _context;

        public ArtworkRepository(GalleryDataContext context)
        {
            _context = context;
        }

        public List<Artwork> GetAll()
        {
            return _context.Artworks.ToList();
        }

        public List<Artwork> GetByStatus(string status)
        {
            return _context.Artworks
                .Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Artwork> GetByArtistId(int artistId)
        {
            return _context.Artworks
                .Where(a => a.ArtistId == artistId)
                .ToList();
        }

        public Artwork? GetById(int id)
        {
            return _context.Artworks.FirstOrDefault(a => a.Id == id);
        }

        public Artwork Add(Artwork artwork)
        {
            _context.Artworks.Add(artwork);
            _context.SaveChanges();
            return artwork;
        }

        public Artwork Update(Artwork artwork)
        {
            var existingArtwork = GetById(artwork.Id);
            if (existingArtwork != null)
            {
                existingArtwork.Title = artwork.Title;
                existingArtwork.ArtistId = artwork.ArtistId;
                existingArtwork.Medium = artwork.Medium;
                existingArtwork.YearCreated = artwork.YearCreated;
                existingArtwork.Price = artwork.Price;
                existingArtwork.Dimensions = artwork.Dimensions;
                existingArtwork.Status = artwork.Status;
                existingArtwork.Description = artwork.Description;
                _context.SaveChanges();
            }

            return existingArtwork!;
        }

        public void Delete(int id)
        {
            var artwork = GetById(id);
            if (artwork != null)
            {
                _context.Artworks.Remove(artwork);
                _context.SaveChanges();
            }
        }

        public int GetNextId()
        {
            // EF manages the ID automatically
            return _context.Artworks.Any() ? _context.Artworks.Max(a => a.Id) + 1 : 1;
        }
    }
}
