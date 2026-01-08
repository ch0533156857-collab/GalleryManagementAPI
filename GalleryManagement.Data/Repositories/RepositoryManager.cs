using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Data.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly GalleryDataContext _context;

        public IArtistRepository Artists { get; }
        public IArtworkRepository Artworks { get; }
        public IExhibitionRepository Exhibitions { get; }
        public ISaleRepository Sales { get; }

        public RepositoryManager(
            GalleryDataContext context,
            IArtistRepository artistRepository,
            IArtworkRepository artworkRepository,
            IExhibitionRepository exhibitionRepository,
            ISaleRepository saleRepository)
        {
            _context = context;
            Artists = artistRepository;
            Artworks = artworkRepository;
            Exhibitions = exhibitionRepository;
            Sales = saleRepository;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
