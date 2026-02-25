using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IArtistRepository : IRepository<Artist>
    {
        //List<Artist> GetAll();
        Task<List<Artist>> GetByStatusAsync(string status);
        //Artist? GetById(int id);
        //Artist Add(Artist artist);
        //Artist Update(Artist artist);
        //int GetNextId();
    }
}
