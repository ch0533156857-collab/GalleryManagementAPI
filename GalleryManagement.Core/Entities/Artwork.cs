using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Entities
{
    public class Artwork
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int ArtistId { get; set; }
        public string Medium { get; set; } = string.Empty;
        public int YearCreated { get; set; }
        public string Dimensions { get; set; } = string.Empty;
        public string Status { get; set; } = "available";
        public string Description { get; set; } = string.Empty;
        public int Price { get; set; }
    }
}
