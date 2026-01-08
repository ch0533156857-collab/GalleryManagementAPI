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
        private readonly IRepositoryManager _repositoryManager;
        public ArtistService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public List<Artist> GetAllArtists(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                return _repositoryManager.Artists.GetAll().ToList();
            }
            return _repositoryManager.Artists.GetByStatus(status);
        }

        public Artist? GetArtistById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repositoryManager.Artists.GetById(id);
        }

        public Artist CreateArtist(Artist artist)
        {
            if (string.IsNullOrWhiteSpace(artist.Name))
            {
                throw new ArgumentException("שם האמן הוא שדה חובה");
            }
            if (artist.BirthDate > DateTime.Now)
            {
                throw new ArgumentException("תאריך לידה לא יכול להיות בעתיד");
            }

            artist.CreatedAt = DateTime.Now;
            artist.Status = "active";

            var result = _repositoryManager.Artists.Add(artist);
            _repositoryManager.Save();

            return result;
        }

        public Artist UpdateArtist(int id, Artist updatedArtist)
        {
            var existingArtist = _repositoryManager.Artists.GetById(id);
            if (existingArtist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            if (string.IsNullOrWhiteSpace(updatedArtist.Name))
            {
                throw new ArgumentException("שם האמן הוא שדה חובה");
            }

            existingArtist.Name = updatedArtist.Name;
            existingArtist.Biography = updatedArtist.Biography;
            existingArtist.Nationality = updatedArtist.Nationality;
            existingArtist.BirthDate = updatedArtist.BirthDate;
            existingArtist.Style = updatedArtist.Style;
            existingArtist.Status = updatedArtist.Status;

            _repositoryManager.Artists.Update(existingArtist);
            _repositoryManager.Save();

            return existingArtist;
        }

        public Artist UpdateArtistStatus(int id, string status)
        {
            var artist = _repositoryManager.Artists.GetById(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            if (status != "active" && status != "inactive")
            {
                throw new ArgumentException("סטטוס חייב להיות active או inactive");
            }

            artist.Status = status;
            _repositoryManager.Artists.Update(artist);
            _repositoryManager.Save();

            return artist;
        }

        public List<Artwork> GetArtistArtworks(int id)
        {
            var artist = _repositoryManager.Artists.GetById(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            return _repositoryManager.Artworks.GetAll()
                .Where(a => a.ArtistId == id)
                .ToList();
        }
    }
}
