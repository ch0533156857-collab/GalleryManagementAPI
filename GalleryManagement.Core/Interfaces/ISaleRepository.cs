using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface ISaleRepository
    {
        List<Sale> GetAll();
        List<Sale> GetByStatus(String status);
        List<Sale> GetByArtworkId(int ArtworkId);
        Sale? GetById(int id);
        Sale Add(Sale sale);
        Sale Update(Sale sale);
        void Delete(int id);
        int GetNextId();
    }
}
