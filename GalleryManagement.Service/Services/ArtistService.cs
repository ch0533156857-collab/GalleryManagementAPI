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

        public async Task<List<Artist>> GetAllArtistsAsync(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                var artists =  await _repositoryManager.Artists.GetAllAsync();
                return artists.ToList();
            }
            return await _repositoryManager.Artists.GetByStatusAsync(status);
        }

        public async Task<Artist?> GetArtistByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return await _repositoryManager.Artists.GetByIdAsync(id);
        }

        public async Task<Artist> CreateArtistAsync(Artist artist)
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

            var result = await _repositoryManager.Artists.AddAsync(artist);
            await _repositoryManager.SaveAsync();

            return result;
        }

        public async Task<Artist> UpdateArtistAsync(int id, Artist updatedArtist)
        {
            var existingArtist = await _repositoryManager.Artists.GetByIdAsync(id);
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

            await _repositoryManager.Artists.UpdateAsync(existingArtist);
            await _repositoryManager.SaveAsync();

            return existingArtist;
        }

        public async Task<Artist> UpdateArtistStatusAsync(int id, string status)
        {
            var artist = await _repositoryManager.Artists.GetByIdAsync(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            if (status != "active" && status != "inactive")
            {
                throw new ArgumentException("סטטוס חייב להיות active או inactive");
            }

            artist.Status = status;
            await _repositoryManager.Artists.UpdateAsync(artist);
            await _repositoryManager.SaveAsync();

            return artist;
        }

        public async Task<List<Artwork>> GetArtistArtworksAsync(int id)
        {
            var artist = await _repositoryManager.Artists.GetByIdAsync(id);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {id} לא נמצא");
            }

            return await _repositoryManager.Artworks.GetByArtistIdAsync(id);
        }
    }
}
