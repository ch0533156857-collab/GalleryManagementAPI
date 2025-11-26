using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restful_code.Entities;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        private static List<Artwork> _artworks = new List<Artwork>
        {
            new Artwork
            {
                Id = 1,
                Title = "מונה ליזה",
                ArtistId = 1,
                Medium = "שמן על עץ",
                YearCreated = 1503,
                Price = 1000000000,
                Dimensions = "77x53 ס\"מ",
                Status = "sold",
                Description = "ציור מפורסם מאת לאונרדו דה וינצ'י"
            },
            new Artwork
            {
                Id = 2,
                Title = "גרניקה",
                ArtistId = 2,
                Medium = "שמן על קנבס",
                YearCreated = 1937,
                Price = 200000000,
                Dimensions = "349x776 ס\"מ",
                Status = "available",
                Description = "יצירת מופת קוביסטית"
            }
        };

        private static int _nextId = 3;

        // מתודה סטטית לשימוש ב-Controllers אחרים
        public static List<Artwork> GetArtworksByArtistId(int artistId)
        {
            return _artworks.Where(a => a.ArtistId == artistId).ToList();
        }

        // GET: api/artworks
        [HttpGet]
        public ActionResult<IEnumerable<Artwork>> GetAllArtworks(
            [FromQuery] string? status = null,
            [FromQuery] int? artist = null,
            [FromQuery] string? search = null,
            [FromQuery] string? sort = null,
            [FromQuery] string? order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10)
        {
            var artworks = _artworks.AsQueryable();

            // סינון לפי סטטוס
            if (!string.IsNullOrEmpty(status))
            {
                artworks = artworks.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            // סינון לפי אמן
            if (artist.HasValue)
            {
                artworks = artworks.Where(a => a.ArtistId == artist.Value);
            }

            // חיפוש
            if (!string.IsNullOrEmpty(search))
            {
                artworks = artworks.Where(a =>
                    a.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.Medium.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // מיון
            if (!string.IsNullOrEmpty(sort))
            {
                artworks = sort.ToLower() switch
                {
                    "price" => order == "desc" ? artworks.OrderByDescending(a => a.Price) : artworks.OrderBy(a => a.Price),
                    "year" => order == "desc" ? artworks.OrderByDescending(a => a.YearCreated) : artworks.OrderBy(a => a.YearCreated),
                    "title" => order == "desc" ? artworks.OrderByDescending(a => a.Title) : artworks.OrderBy(a => a.Title),
                    _ => artworks.OrderBy(a => a.Id)
                };
            }

            // Pagination
            var totalItems = artworks.Count();
            var items = artworks.Skip((page - 1) * limit).Take(limit).ToList();

            var response = new
            {
                totalItems,
                page,
                limit,
                totalPages = (int)Math.Ceiling(totalItems / (double)limit),
                items
            };

            return Ok(response);
        }

        // GET: api/artworks/5
        [HttpGet("{id}")]
        public ActionResult<Artwork> GetArtwork(int id)
        {
            var artwork = _artworks.FirstOrDefault(a => a.Id == id);

            if (artwork == null)
            {
                return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
            }

            return Ok(artwork);
        }

        // POST: api/artworks
        [HttpPost]
        public ActionResult<Artwork> CreateArtwork([FromBody] Artwork artwork)
        {
            artwork.Id = _nextId++;
            artwork.CreatedAt = DateTime.Now;
            artwork.Status = "available"; // ברירת מחדל

            _artworks.Add(artwork);

            return CreatedAtAction(nameof(GetArtwork), new { id = artwork.Id }, artwork);
        }

        // PUT: api/artworks/5
        [HttpPut("{id}")]
        public ActionResult<Artwork> UpdateArtwork(int id, [FromBody] Artwork updatedArtwork)
        {
            var artwork = _artworks.FirstOrDefault(a => a.Id == id);

            if (artwork == null)
            {
                return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
            }

            artwork.Title = updatedArtwork.Title;
            artwork.ArtistId = updatedArtwork.ArtistId;
            artwork.Medium = updatedArtwork.Medium;
            artwork.YearCreated = updatedArtwork.YearCreated;
            artwork.Price = updatedArtwork.Price;
            artwork.Dimensions = updatedArtwork.Dimensions;
            artwork.Status = updatedArtwork.Status;
            artwork.ImageUrl = updatedArtwork.ImageUrl;
            artwork.Description = updatedArtwork.Description;

            return Ok(artwork);
        }

        // DELETE: api/artworks/5
        [HttpDelete("{id}")]
        public ActionResult DeleteArtwork(int id)
        {
            var artwork = _artworks.FirstOrDefault(a => a.Id == id);

            if (artwork == null)
            {
                return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
            }

            _artworks.Remove(artwork);

            return Ok(new { message = "היצירה נמחקה בהצלחה", deletedId = id });
        }

        // PATCH: api/artworks/5/status
        [HttpPatch("{id}/status")]
        public ActionResult<Artwork> UpdateArtworkStatus(int id, [FromBody] ArtworkStatusUpdate statusUpdate)
        {
            var artwork = _artworks.FirstOrDefault(a => a.Id == id);

            if (artwork == null)
            {
                return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
            }

            var validStatuses = new[] { "available", "sold", "on_loan", "reserved" };
            if (!validStatuses.Contains(statusUpdate.Status.ToLower()))
            {
                return BadRequest(new { message = $"סטטוס לא תקין. ערכים אפשריים: {string.Join(", ", validStatuses)}" });
            }

            artwork.Status = statusUpdate.Status.ToLower();

            return Ok(artwork);
        }
    }

    public class ArtworkStatusUpdate
    {
        public string Status { get; set; } = string.Empty;
    }
}