using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;

namespace GalleryManagement.Service.Services
{
    public class ArtworkService : IArtworkService
    {
        private readonly IArtworkRepository _repository;
        private readonly IArtistRepository _artistRepository;

        public ArtworkService(IArtworkRepository repository, IArtistRepository artistRepository)
        {
            _repository = repository;
            _artistRepository = artistRepository;
        }

        public List<Artwork> GetAllArtworks(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                return _repository.GetAll();
            }
            return _repository.GetByStatus(status);
        }

        public List<Artwork> GetArtworksByArtist(int artistId)
        {
            var artist = _artistRepository.GetById(artistId);
            if (artist == null)
            {
                throw new KeyNotFoundException($"אמן עם מזהה {artistId} לא נמצא");
            }
            return _repository.GetByArtistId(artistId);
        }

        public Artwork? GetArtworkById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repository.GetById(id);
        }

        public Artwork CreateArtwork(Artwork artwork)
        {
            if (string.IsNullOrWhiteSpace(artwork.Title))
            {
                throw new ArgumentException("כותרת היצירה היא שדה חובה");
            }

            var artist = _artistRepository.GetById(artwork.ArtistId);
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
            return _repository.Add(artwork);
        }

        public Artwork UpdateArtwork(int id, Artwork updatedArtwork)
        {
            var existingArtwork = _repository.GetById(id);
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

            updatedArtwork.Id = id;
            return _repository.Update(updatedArtwork);
        }

        public Artwork UpdateArtworkStatus(int id, string status)
        {
            var artwork = _repository.GetById(id);
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
            return _repository.Update(artwork);
        }

        public void DeleteArtwork(int id)
        {
            var artwork = _repository.GetById(id);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {id} לא נמצאה");
            }

            _repository.Delete(id);
        }
    }
}