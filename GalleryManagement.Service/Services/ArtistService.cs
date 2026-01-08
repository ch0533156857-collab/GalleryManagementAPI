using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Service.Services
{
    public class ArtistService : IArtistService
    {
        private readonly IArtistRepository _repository;
        private readonly GalleryDataContext _context;

        public ArtistService(IArtistRepository repository, GalleryDataContext context)
        {
            _repository = repository;
            _context = context;
        }

        public List<Artist> GetAllArtists(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                return _repository.GetAll();
            }
            return _repository.GetByStatus(status);
        }

        public Artist? GetArtistById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repository.GetById(id);
        }

        public Artist CreateArtist(Artist artist)
        {
            // Validations - בדיקות תקינות
            if (string.IsNullOrWhiteSpace(artist.Name))
            {
                throw new ArgumentException("שם האמן הוא שדה חובה");
            }

            if (artist.BirthDate > DateTime.Now)
            {
                throw new ArgumentException("תאריך לידה לא יכול להיות בעתיד");
            }

            return _repository.Add(artist);
        }

        public Artist UpdateArtist(int id, Artist updatedArtist)
        {
            var existingArtist = _repository.GetById(id);
            if (existingArtist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            if (string.IsNullOrWhiteSpace(updatedArtist.Name))
            {
                throw new ArgumentException("שם האמן הוא שדה חובה");
            }

            updatedArtist.Id = id;
            return _repository.Update(updatedArtist);
        }

        public Artist UpdateArtistStatus(int id, string status)
        {
            var artist = _repository.GetById(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            if (status != "active" && status != "inactive")
            {
                throw new ArgumentException("סטטוס חייב להיות active או inactive");
            }

            artist.Status = status;
            return _repository.Update(artist);
        }

        public List<Artwork> GetArtistArtworks(int id)
        {
            var artist = _repository.GetById(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            return _context.Artworks.Where(a => a.ArtistId == id).ToList();
        }
    }
}
