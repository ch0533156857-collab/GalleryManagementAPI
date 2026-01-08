using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Interfaces
{
    public interface IRepositoryManager
    {
        IArtistRepository Artists { get; }
        IArtworkRepository Artworks { get; }
        ISaleRepository Sales { get; }
        IExhibitionRepository Exhibitions { get; }

        void Save();
    }
}
