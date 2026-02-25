using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface ISaleService
    {
        Task<List<Sale>> GetAllSalesAsync(string? status = null);
        Task<List<Sale>> GetSalesByArtworkAsync(int artworkId);
        Task<Sale?> GetSaleByIdAsync(int id);
        Task<Sale> CreateSaleAsync(Sale sale);
        Task<Sale> UpdateSaleAsync(int id, Sale updatedSale);
        Task<Sale> UpdateSaleStatusAsync(int id, string status);
        Task DeleteSaleAsync(int id);
    }
}
