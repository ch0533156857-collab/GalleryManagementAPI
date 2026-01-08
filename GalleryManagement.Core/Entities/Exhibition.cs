using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalleryManagement.Core.Entities
{
    public class Exhibition
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string CuratorName { get; set; } = string.Empty;
        public string ArtworkIdsJsin { get; set; } = "[]";

        [NotMapped]
        public List<int> ArtworkIds
        {
            get => System.Text.Json.JsonSerializer.Deserialize<List<int>>(ArtworkIdsJsin) ?? new List<int>();
            set => ArtworkIdsJsin = System.Text.Json.JsonSerializer.Serialize(value);
        }

    }
}
