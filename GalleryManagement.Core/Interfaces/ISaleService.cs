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
        List<Sale> GetAllSales(string? status = null);
        List<Sale> GetSalesByArtwork(int artworkId);
        Sale? GetSaleById(int id);
        Sale CreateSale(Sale sale);
        Sale UpdateSale(int id, Sale updatedSale);
        Sale UpdateSaleStatus(int id, string status);
        void DeleteSale(int id);
    }
}
