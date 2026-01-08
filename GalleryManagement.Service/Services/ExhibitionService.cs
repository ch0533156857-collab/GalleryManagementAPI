using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;

namespace GalleryManagement.Service.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly IExhibitionRepository _repository;
        private readonly IArtworkRepository _artworkRepository;

        public ExhibitionService(IExhibitionRepository repository, IArtworkRepository artworkRepository)
        {
            _repository = repository;
            _artworkRepository = artworkRepository;
        }

        public List<Exhibition> GetAllExhibitions()
        {
            return _repository.GetAll();
        }

        public List<Exhibition> GetActiveExhibitions()
        {
            return _repository.GetActive();
        }

        public Exhibition? GetExhibitionById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repository.GetById(id);
        }

        public Exhibition CreateExhibition(Exhibition exhibition)
        {
            if (string.IsNullOrWhiteSpace(exhibition.Name))
            {
                throw new ArgumentException("שם התערוכה הוא שדה חובה");
            }

            if (exhibition.StartDate >= exhibition.EndDate)
            {
                throw new ArgumentException("תאריך סיום חייב להיות אחרי תאריך התחלה");
            }

            if (exhibition.StartDate < DateTime.Now.Date)
            {
                throw new ArgumentException("תאריך התחלה לא יכול להיות בעבר");
            }

            return _repository.Add(exhibition);
        }

        public Exhibition UpdateExhibition(int id, Exhibition updatedExhibition)
        {
            var existingExhibition = _repository.GetById(id);
            if (existingExhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {id} לא נמצאה");
            }

            if (string.IsNullOrWhiteSpace(updatedExhibition.Name))
            {
                throw new ArgumentException("שם התערוכה הוא שדה חובה");
            }

            if (updatedExhibition.StartDate >= updatedExhibition.EndDate)
            {
                throw new ArgumentException("תאריך סיום חייב להיות אחרי תאריך התחלה");
            }

            updatedExhibition.Id = id;
            return _repository.Update(updatedExhibition);
        }

        public Exhibition AddArtworkToExhibition(int exhibitionId, int artworkId)
        {
            var exhibition = _repository.GetById(exhibitionId);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {exhibitionId} לא נמצאה");
            }

            var artwork = _artworkRepository.GetById(artworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {artworkId} לא נמצאה");
            }

            if (exhibition.ArtworkIds.Contains(artworkId))
            {
                throw new InvalidOperationException("היצירה כבר קיימת בתערוכה");
            }

            exhibition.ArtworkIds.Add(artworkId);
            return _repository.Update(exhibition);
        }

        public Exhibition RemoveArtworkFromExhibition(int exhibitionId, int artworkId)
        {
            var exhibition = _repository.GetById(exhibitionId);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {exhibitionId} לא נמצאה");
            }

            if (!exhibition.ArtworkIds.Contains(artworkId))
            {
                throw new InvalidOperationException("היצירה לא קיימת בתערוכה");
            }

            exhibition.ArtworkIds.Remove(artworkId);
            return _repository.Update(exhibition);
        }

        public void DeleteExhibition(int id)
        {
            var exhibition = _repository.GetById(id);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {id} לא נמצאה");
            }

            _repository.Delete(id);
        }
    }
}