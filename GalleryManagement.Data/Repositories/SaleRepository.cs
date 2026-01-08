using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;

namespace GalleryManagement.Data.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly GalleryDataContext _context;

        public SaleRepository(GalleryDataContext context)
        {
            _context = context;
        }

        public List<Sale> GetAll()
        {
            return _context.Sales.ToList();
        }

        public List<Sale> GetByStatus(string status)
        {
            return _context.Sales
                .Where(s => s.Status.Equals(status, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public List<Sale> GetByArtworkId(int artworkId)
        {
            return _context.Sales
                .Where(s => s.ArtworkId == artworkId)
                .ToList();
        }

        public Sale? GetById(int id)
        {
            return _context.Sales.FirstOrDefault(s => s.Id == id);
        }

        public Sale Add(Sale sale)
        {
            sale.SaleDate = DateTime.Now;
            _context.Sales.Add(sale);
            _context.SaveChanges();
            return sale;
        }

        public Sale Update(Sale sale)
        {
            var existingSale = GetById(sale.Id);
            if (existingSale != null)
            {
                existingSale.ArtworkId = sale.ArtworkId;
                existingSale.BuyerName = sale.BuyerName;
                existingSale.BuyerEmail = sale.BuyerEmail;
                existingSale.SalePrice = sale.SalePrice;
                existingSale.PaymentMethod = sale.PaymentMethod;
                existingSale.Status = sale.Status;
                _context.SaveChanges(); 
            }
            return existingSale!;
        }

        public void Delete(int id)
        {
            var sale = GetById(id);
            if (sale != null)
            {
                _context.Sales.Remove(sale);
                _context.SaveChanges();
            }
        }

        public int GetNextId()
        {
            // EF מנהל את ה-ID אוטומטית
            return _context.Sales.Any() ? _context.Sales.Max(s => s.Id) + 1 : 1;
        }
    }


}