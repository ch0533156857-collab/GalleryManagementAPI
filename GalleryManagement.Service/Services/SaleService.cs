using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;

namespace GalleryManagement.Service.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository _repository;
        private readonly IArtworkRepository _artworkRepository;

        public SaleService(ISaleRepository repository, IArtworkRepository artworkRepository)
        {
            _repository = repository;
            _artworkRepository = artworkRepository;
        }

        public List<Sale> GetAllSales(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                return _repository.GetAll();
            }
            return _repository.GetByStatus(status);
        }

        public List<Sale> GetSalesByArtwork(int artworkId)
        {
            var artwork = _artworkRepository.GetById(artworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {artworkId} לא נמצאה");
            }
            return _repository.GetByArtworkId(artworkId);
        }

        public Sale? GetSaleById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repository.GetById(id);
        }

        public Sale CreateSale(Sale sale)
        {
            if (string.IsNullOrWhiteSpace(sale.BuyerName))
            {
                throw new ArgumentException("שם הקונה הוא שדה חובה");
            }

            if (string.IsNullOrWhiteSpace(sale.BuyerEmail))
            {
                throw new ArgumentException("אימייל הקונה הוא שדה חובה");
            }

            var artwork = _artworkRepository.GetById(sale.ArtworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {sale.ArtworkId} לא נמצאה");
            }

            if (artwork.Status == "sold")
            {
                throw new InvalidOperationException("היצירה כבר נמכרה");
            }

            if (sale.SalePrice <= 0)
            {
                throw new ArgumentException("מחיר מכירה חייב להיות חיובי");
            }

            sale.Status = "pending";
            var createdSale = _repository.Add(sale);

            // עדכן את סטטוס היצירה
            artwork.Status = "sold";
            _artworkRepository.Update(artwork);

            return createdSale;
        }

        public Sale UpdateSale(int id, Sale updatedSale)
        {
            var existingSale = _repository.GetById(id);
            if (existingSale == null)
            {
                throw new KeyNotFoundException($"מכירה עם מזהה {id} לא נמצאה");
            }

            if (string.IsNullOrWhiteSpace(updatedSale.BuyerName))
            {
                throw new ArgumentException("שם הקונה הוא שדה חובה");
            }

            if (updatedSale.SalePrice <= 0)
            {
                throw new ArgumentException("מחיר מכירה חייב להיות חיובי");
            }

            updatedSale.Id = id;
            return _repository.Update(updatedSale);
        }

        public Sale UpdateSaleStatus(int id, string status)
        {
            var sale = _repository.GetById(id);
            if (sale == null)
            {
                throw new KeyNotFoundException($"מכירה עם מזהה {id} לא נמצאה");
            }

            var validStatuses = new[] { "pending", "completed", "cancelled" };
            if (!validStatuses.Contains(status.ToLower()))
            {
                throw new ArgumentException($"סטטוס חייב להיות אחד מהבאים: {string.Join(", ", validStatuses)}");
            }

            sale.Status = status;
            return _repository.Update(sale);
        }

        public void DeleteSale(int id)
        {
            var sale = _repository.GetById(id);
            if (sale == null)
            {
                throw new KeyNotFoundException($"מכירה עם מזהה {id} לא נמצאה");
            }

            _repository.Delete(id);
        }
    }
}