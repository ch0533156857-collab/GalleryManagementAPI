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

        public async Task<List<Exhibition>> GetAllExhibitionsAsync()
        {
            return (await _repositoryManager.Exhibitions.GetAllAsync()).ToList();
        }

        public async Task<List<Exhibition>> GetActiveExhibitionsAsync()
        {
            return await _repositoryManager.Exhibitions.GetActiveAsync();
        }

        public async Task<Exhibition?> GetExhibitionByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return await _repositoryManager.Exhibitions.GetByIdAsync(id);
        }

        public async Task<Exhibition> CreateExhibitionAsync(Exhibition exhibition)
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

            var result = await _repositoryManager.Exhibitions.AddAsync(exhibition);
            await _repositoryManager.SaveAsync();

            return result;
        }

        public async Task<Exhibition> UpdateExhibitionAsync(int id, Exhibition updatedExhibition)
        {
            var existingExhibition = await _repositoryManager.Exhibitions.GetByIdAsync(id);
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

            await _repositoryManager.Exhibitions.UpdateAsync(existingExhibition);
            await _repositoryManager.SaveAsync();

            return existingExhibition;
        }

        public async Task<Exhibition> AddArtworkToExhibitionAsync(int exhibitionId, int artworkId)
        {
            var exhibition = await _repositoryManager.Exhibitions.GetByIdAsync(exhibitionId);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {exhibitionId} לא נמצאה");
            }

            var artwork = _repositoryManager.Artworks.GetByIdAsync(artworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {artworkId} לא נמצאה");
            }

            if (exhibition.ArtworkIds.Contains(artworkId))
            {
                throw new InvalidOperationException("היצירה כבר קיימת בתערוכה");
            }

            exhibition.ArtworkIds.Add(artworkId);
            await _repositoryManager.Exhibitions.UpdateAsync(exhibition);
            await _repositoryManager.SaveAsync();

            return exhibition;
        }

        public async Task<Exhibition> RemoveArtworkFromExhibitionAsync(int exhibitionId, int artworkId)
        {
            var exhibition = await _repositoryManager.Exhibitions.GetByIdAsync(exhibitionId);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {exhibitionId} לא נמצאה");
            }

            if (!exhibition.ArtworkIds.Contains(artworkId))
            {
                throw new InvalidOperationException("היצירה לא קיימת בתערוכה");
            }

            exhibition.ArtworkIds.Remove(artworkId);
            await _repositoryManager.Exhibitions.UpdateAsync(exhibition);
            await _repositoryManager.SaveAsync();

            return exhibition;
        }

        public async Task DeleteExhibitionAsync(int id)
        {
            var exhibition = await _repositoryManager.Exhibitions.GetByIdAsync(id);
            if (exhibition == null)
            {
                throw new KeyNotFoundException($"תערוכה עם מזהה {id} לא נמצאה");
            }

            await _repositoryManager.Exhibitions.DeleteAsync(exhibition);
            await _repositoryManager.SaveAsync();
        }
    }
}