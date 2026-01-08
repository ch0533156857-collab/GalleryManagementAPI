using GalleryManagement.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IExhibitionRepository : IRepository<Exhibition>
    {
        List<Exhibition> GetActive();

    }
}
