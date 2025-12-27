using Microsoft.AspNetCore.Mvc;
using restful_code.Data;
using restful_code.Entities;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        private readonly DataContext _context;

        public ArtworksController(DataContext context)
        {
            _context = context;
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
            var artworks = _context.Artworks.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                artworks = artworks.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            if (artist.HasValue)
            {
                artworks = artworks.Where(a => a.ArtistId == artist.Value);
            }

            if (!string.IsNullOrEmpty(search))
            {
                artworks = artworks.Where(a =>
                    a.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.Medium.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.Description.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

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
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);

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
            artwork.Id = _context.NextArtworkId++;
            artwork.CreatedAt = DateTime.Now;
            artwork.Status = "available";

            _context.Artworks.Add(artwork);

            return CreatedAtAction(nameof(GetArtwork), new { id = artwork.Id }, artwork);
        }

        // PUT: api/artworks/5
        [HttpPut("{id}")]
        public ActionResult<Artwork> UpdateArtwork(int id, [FromBody] Artwork updatedArtwork)
        {
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);

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
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);

            if (artwork == null)
            {
                return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
            }

            _context.Artworks.Remove(artwork);

            return Ok(new { message = "היצירה נמחקה בהצלחה", deletedId = id });
        }

        // PATCH: api/artworks/5/status
        [HttpPatch("{id}/status")]
        public ActionResult<Artwork> UpdateArtworkStatus(int id, [FromBody] ArtworkStatusUpdate statusUpdate)
        {
            var artwork = _context.Artworks.FirstOrDefault(a => a.Id == id);

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