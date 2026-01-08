using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;

namespace GalleryManagement.Service.Services
{
    public class SaleService : ISaleService
    {
        private readonly IRepositoryManager _repositoryManager;
        public SaleService(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public List<Sale> GetAllSales(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                return _repositoryManager.Sales.GetAll().ToList();
            }
            return _repositoryManager.Sales.GetByStatus(status);
        }

        public List<Sale> GetSalesByArtwork(int artworkId)
        {
            var artwork = _repositoryManager.Artworks.GetById(artworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {artworkId} לא נמצאה");
            }
            return _repositoryManager.Sales.GetByArtworkId(artworkId);
        }

        public Sale? GetSaleById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return _repositoryManager.Sales.GetById(id);
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

            var artwork = _repositoryManager.Artworks.GetById(sale.ArtworkId);
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
            sale.SaleDate = DateTime.Now;

            // הוספת המכירה
            var createdSale = _repositoryManager.Sales.Add(sale);

            // עדכון סטטוס היצירה
            artwork.Status = "sold";
            _repositoryManager.Artworks.Update(artwork);

            // שמירה של שתי הפעולות ביחד - טרנזקציה!
            _repositoryManager.Save();

            return createdSale;
        }

        public Sale UpdateSale(int id, Sale updatedSale)
        {
            var existingSale = _repositoryManager.Sales.GetById(id);
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

            existingSale.ArtworkId = updatedSale.ArtworkId;
            existingSale.BuyerName = updatedSale.BuyerName;
            existingSale.BuyerEmail = updatedSale.BuyerEmail;
            existingSale.SalePrice = updatedSale.SalePrice;
            existingSale.PaymentMethod = updatedSale.PaymentMethod;
            existingSale.Status = updatedSale.Status;

            _repositoryManager.Sales.Update(existingSale);
            _repositoryManager.Save();

            return existingSale;
        }

        public Sale UpdateSaleStatus(int id, string status)
        {
            var sale = _repositoryManager.Sales.GetById(id);
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
            _repositoryManager.Sales.Update(sale);
            _repositoryManager.Save();

            return sale;
        }

        public void DeleteSale(int id)
        {
            var sale = _repositoryManager.Sales.GetById(id);
            if (sale == null)
            {
                throw new KeyNotFoundException($"מכירה עם מזהה {id} לא נמצאה");
            }

            _repositoryManager.Sales.Delete(sale);
            _repositoryManager.Save();
        }
    }
}