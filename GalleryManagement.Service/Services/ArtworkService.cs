using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;

namespace GalleryManagement.Service.Services
{
    public class ArtworkService : IArtworkService
    {
            private readonly IRepositoryManager _repositoryManager;

            public ArtworkService(IRepositoryManager repositoryManager)
            {
                _repositoryManager = repositoryManager;
            }

            public async Task<List<Artwork>> GetAllArtworksAsync(string? status = null)
            {
                if (string.IsNullOrEmpty(status))
                {
                    var artworks = await _repositoryManager.Artworks.GetAllAsync();
                    return artworks.ToList();
            }
                return await _repositoryManager.Artworks.GetByStatusAsync(status);
            }

            public async Task<List<Artwork>> GetArtworksByArtistAsync(int artistId)
            {
                var artist = await _repositoryManager.Artists.GetByIdAsync(artistId);
                if (artist == null)
                {
                    throw new KeyNotFoundException($"אמן עם מזהה {artistId} לא נמצא");
                }
                return await _repositoryManager.Artworks.GetByArtistIdAsync(artistId);
            }

            public async Task<Artwork?> GetArtworkByIdAsync(int id)
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID חייב להיות חיובי");
                }
                return await _repositoryManager.Artworks.GetByIdAsync(id);
            }

            public async Task<Artwork> CreateArtworkAsync(Artwork artwork)
            {
                if (string.IsNullOrWhiteSpace(artwork.Title))
                {
                    throw new ArgumentException("כותרת היצירה היא שדה חובה");
                }

                var artist = await _repositoryManager.Artists.GetByIdAsync(artwork.ArtistId);
                if (artist == null)
                {
                    throw new KeyNotFoundException($"אמן עם מזהה {artwork.ArtistId} לא נמצא");
                }

                if (artwork.Price < 0)
                {
                    throw new ArgumentException("מחיר לא יכול להיות שלילי");
                }

                if (artwork.YearCreated > DateTime.Now.Year)
                {
                    throw new ArgumentException("שנת יצירה לא יכולה להיות בעתיד");
                }

                artwork.Status = "available";
                var result = await _repositoryManager.Artworks.AddAsync(artwork);
                await _repositoryManager.SaveAsync();

                return result;
            }

            public async Task<Artwork> UpdateArtworkAsync(int id, Artwork updatedArtwork)
            {
                var existingArtwork = await _repositoryManager.Artworks.GetByIdAsync(id);
                if (existingArtwork == null)
                {
                    throw new KeyNotFoundException($"יצירה עם מזהה {id} לא נמצאה");
                }

                if (string.IsNullOrWhiteSpace(updatedArtwork.Title))
                {
                    throw new ArgumentException("כותרת היצירה היא שדה חובה");
                }

                if (updatedArtwork.Price < 0)
                {
                    throw new ArgumentException("מחיר לא יכול להיות שלילי");
                }

                existingArtwork.Title = updatedArtwork.Title;
                existingArtwork.ArtistId = updatedArtwork.ArtistId;
                existingArtwork.Medium = updatedArtwork.Medium;
                existingArtwork.YearCreated = updatedArtwork.YearCreated;
                existingArtwork.Price = updatedArtwork.Price;
                existingArtwork.Dimensions = updatedArtwork.Dimensions;
                existingArtwork.Status = updatedArtwork.Status;
                existingArtwork.Description = updatedArtwork.Description;

                await _repositoryManager.Artworks.UpdateAsync(existingArtwork);
                await _repositoryManager.SaveAsync();

                return existingArtwork;
            }

            public async Task<Artwork> UpdateArtworkStatusAsync(int id, string status)
            {
                var artwork = await _repositoryManager.Artworks.GetByIdAsync(id);
                if (artwork == null)
                {
                    throw new KeyNotFoundException($"יצירה עם מזהה {id} לא נמצאה");
                }

                var validStatuses = new[] { "available", "sold", "reserved", "exhibition" };
                if (!validStatuses.Contains(status.ToLower()))
                {
                    throw new ArgumentException($"סטטוס חייב להיות אחד מהבאים: {string.Join(", ", validStatuses)}");
                }

                artwork.Status = status;
                await _repositoryManager.Artworks.UpdateAsync(artwork);
                await _repositoryManager.SaveAsync();

                return artwork;
            }

            public async Task DeleteArtworkAsync(int id)
            {
                var artwork = await _repositoryManager.Artworks.GetByIdAsync(id);
                if (artwork == null)
                {
                    throw new KeyNotFoundException($"יצירה עם מזהה {id} לא נמצאה");
                }

                await _repositoryManager.Artworks.DeleteAsync(artwork);
                await _repositoryManager.SaveAsync();
            }
        }
}