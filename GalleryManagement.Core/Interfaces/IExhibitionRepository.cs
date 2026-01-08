using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IExhibitionRepository
    {
        List<Exhibition> GetAll();
        List<Exhibition> GetActive();
        Exhibition? GetById(int id);
        Exhibition Add(Exhibition exhibition);
        Exhibition Update(Exhibition exhibition);
        void Delete(int id);
        int GetNextId();
    }
}
