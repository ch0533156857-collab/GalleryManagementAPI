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

        public async Task<List<Sale>> GetAllSalesAsync(string? status = null)
        {
            if (string.IsNullOrEmpty(status))
            {
                var sales = await _repositoryManager.Sales.GetAllAsync();
                return sales.ToList();
            }
            return await _repositoryManager.Sales.GetByStatusAsync(status);
        }

        public async Task<List<Sale>> GetSalesByArtworkAsync(int artworkId)
        {
            var artwork = await _repositoryManager.Artworks.GetByIdAsync(artworkId);
            if (artwork == null)
            {
                throw new KeyNotFoundException($"יצירה עם מזהה {artworkId} לא נמצאה");
            }
            return await _repositoryManager.Sales.GetByArtworkIdAsync(artworkId);
        }

        public async Task<Sale?> GetSaleByIdAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID חייב להיות חיובי");
            }
            return await _repositoryManager.Sales.GetByIdAsync(id);
        }

        public async Task<Sale> CreateSaleAsync(Sale sale)
        {
            if (string.IsNullOrWhiteSpace(sale.BuyerName))
            {
                throw new ArgumentException("שם הקונה הוא שדה חובה");
            }

            if (string.IsNullOrWhiteSpace(sale.BuyerEmail))
            {
                throw new ArgumentException("אימייל הקונה הוא שדה חובה");
            }

            var artwork = await _repositoryManager.Artworks.GetByIdAsync(sale.ArtworkId);
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
            var createdSale = await _repositoryManager.Sales.AddAsync(sale);

            // עדכון סטטוס היצירה
            artwork.Status = "sold";
            await _repositoryManager.Artworks.UpdateAsync(artwork);

            // שמירה של שתי הפעולות ביחד - טרנזקציה!
            await _repositoryManager.SaveAsync();

            return createdSale;
        }

        public async Task<Sale> UpdateSaleAsync(int id, Sale updatedSale)
        {
            var existingSale = await _repositoryManager.Sales.GetByIdAsync(id);
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

            await _repositoryManager.Sales.UpdateAsync(existingSale);
            await _repositoryManager.SaveAsync();

            return existingSale;
        }

        public async Task<Sale> UpdateSaleStatusAsync(int id, string status)
        {
            var sale = await _repositoryManager.Sales.GetByIdAsync(id);
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
            await _repositoryManager.Sales.UpdateAsync(sale);
            await _repositoryManager.SaveAsync();

            return sale;
        }

        public async Task DeleteSaleAsync(int id)
        {
            var sale = await _repositoryManager.Sales.GetByIdAsync(id);
            if (sale == null)
            {
                throw new KeyNotFoundException($"מכירה עם מזהה {id} לא נמצאה");
            }

            await _repositoryManager.Sales.DeleteAsync(sale);
            await _repositoryManager.SaveAsync();
        }
    }
}