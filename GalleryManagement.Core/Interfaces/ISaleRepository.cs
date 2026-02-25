using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{ 
    public interface ISaleRepository : IRepository<Sale>
    {
        Task<List<Sale>> GetByStatusAsync(string status);
        Task<List<Sale>> GetByArtworkIdAsync(int artworkId);
    }
}
