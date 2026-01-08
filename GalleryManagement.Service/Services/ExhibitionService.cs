using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;

namespace GalleryManagement.Service.Services
{
    public class ExhibitionService : IExhibitionService
    {
        private readonly IRepositoryManager _repositoryManager;

        public ExhibitionService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public List<Exhibition> GetAllExhibitions()
        {
            return _repositoryManager.Exhibitions.GetAll().ToList();
        }

        public List<Exhibition> GetActiveExhibitions()
        {
            return _repositoryManager.Exhibitions.GetActive();
        }

        public Exhibition? GetExhibitionById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repositoryManager.Exhibitions.GetById(id);
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

            var result = _repositoryManager.Exhibitions.Add(exhibition);
            _repositoryManager.Save();

            return result;
        }

        public Exhibition UpdateExhibition(int id, Exhibition updatedExhibition)
        {
            var existingExhibition = _repositoryManager.Exhibitions.GetById(id);
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

            existingExhibition.Name = updatedExhibition.Name;
            existingExhibition.Description = updatedExhibition.Description;
            existingExhibition.StartDate = updatedExhibition.StartDate;
            existingExhibition.EndDate = updatedExhibition.EndDate;
            existingExhibition.Location = updatedExhibition.Location;
            existingExhibition.CuratorName = updatedExhibition.CuratorName;
            existingExhibition.ArtworkIds = updatedExhibition.ArtworkIds;

            _repositoryManager.Exhibitions.Update(existingExhibition);
            _repositoryManager.Save();

            return existingExhibition;
        }

        public Exhibition AddArtworkToExhibition(int exhibitionId, int artworkId)
        {
            var exhibition = _repositoryManager.Exhibitions.GetById(exhibitionId);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {exhibitionId} לא נמצאה");
            }

            var artwork = _repositoryManager.Artworks.GetById(artworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {artworkId} לא נמצאה");
            }

            if (exhibition.ArtworkIds.Contains(artworkId))
            {
                throw new InvalidOperationException("היצירה כבר קיימת בתערוכה");
            }

            exhibition.ArtworkIds.Add(artworkId);
            _repositoryManager.Exhibitions.Update(exhibition);
            _repositoryManager.Save();

            return exhibition;
        }

        public Exhibition RemoveArtworkFromExhibition(int exhibitionId, int artworkId)
        {
            var exhibition = _repositoryManager.Exhibitions.GetById(exhibitionId);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {exhibitionId} לא נמצאה");
            }

            if (!exhibition.ArtworkIds.Contains(artworkId))
            {
                throw new InvalidOperationException("היצירה לא קיימת בתערוכה");
            }

            exhibition.ArtworkIds.Remove(artworkId);
            _repositoryManager.Exhibitions.Update(exhibition);
            _repositoryManager.Save();

            return exhibition;
        }

        public void DeleteExhibition(int id)
        {
            var exhibition = _repositoryManager.Exhibitions.GetById(id);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {id} לא נמצאה");
            }

            _repositoryManager.Exhibitions.Delete(exhibition);
            _repositoryManager.Save();
        }
    }
}