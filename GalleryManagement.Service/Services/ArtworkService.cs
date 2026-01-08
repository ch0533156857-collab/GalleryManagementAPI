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

            public List<Artwork> GetAllArtworks(string? status = null)
            {
                if (string.IsNullOrEmpty(status))
                {
                    return _repositoryManager.Artworks.GetAll().ToList();
                }
                return _repositoryManager.Artworks.GetByStatus(status);
            }

            public List<Artwork> GetArtworksByArtist(int artistId)
            {
                var artist = _repositoryManager.Artists.GetById(artistId);
                if (artist == null)
                {
                    throw new KeyNotFoundException($"אמן עם מזהה {artistId} לא נמצא");
                }
                return _repositoryManager.Artworks.GetByArtistId(artistId);
            }

            public Artwork? GetArtworkById(int id)
            {
                if (id <= 0)
                {
                    throw new ArgumentException("ID חייב להיות חיובי");
                }
                return _repositoryManager.Artworks.GetById(id);
            }

            public Artwork CreateArtwork(Artwork artwork)
            {
                if (string.IsNullOrWhiteSpace(artwork.Title))
                {
                    throw new ArgumentException("כותרת היצירה היא שדה חובה");
                }

                var artist = _repositoryManager.Artists.GetById(artwork.ArtistId);
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
                var result = _repositoryManager.Artworks.Add(artwork);
                _repositoryManager.Save();

                return result;
            }

            public Artwork UpdateArtwork(int id, Artwork updatedArtwork)
            {
                var existingArtwork = _repositoryManager.Artworks.GetById(id);
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

                _repositoryManager.Artworks.Update(existingArtwork);
                _repositoryManager.Save();

                return existingArtwork;
            }

            public Artwork UpdateArtworkStatus(int id, string status)
            {
                var artwork = _repositoryManager.Artworks.GetById(id);
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
                _repositoryManager.Artworks.Update(artwork);
                _repositoryManager.Save();

                return artwork;
            }

            public void DeleteArtwork(int id)
            {
                var artwork = _repositoryManager.Artworks.GetById(id);
                if (artwork == null)
                {
                    throw new KeyNotFoundException($"יצירה עם מזהה {id} לא נמצאה");
                }

                _repositoryManager.Artworks.Delete(artwork);
                _repositoryManager.Save();
            }
        }
}