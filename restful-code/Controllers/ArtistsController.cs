using restful_code.Data;
using restful_code.Entities;
using Microsoft.AspNetCore.Mvc;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly DataContext _context;

        // 🎯 Constructor - מקבל את DataContext בהזרקה
        public ArtistsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/artists
        [HttpGet]
        public ActionResult<IEnumerable<Artist>> GetAllArtists([FromQuery] string? status = null)
        {
            var artists = _context.Artists.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                artists = artists.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }

            return Ok(artists.ToList());
        }

        // GET: api/artists/5
        [HttpGet("{id}")]
        public ActionResult<Artist> GetArtist(int id)
        {
            var artist = _context.Artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            return Ok(artist);
        }

        // POST: api/artists
        [HttpPost]
        public ActionResult<Artist> CreateArtist([FromBody] Artist artist)
        {
            artist.Id = _context.NextArtistId++;
            artist.CreatedAt = DateTime.Now;
            artist.Status = "active";

            _context.Artists.Add(artist);

            return CreatedAtAction(nameof(GetArtist), new { id = artist.Id }, artist);
        }

        // PUT: api/artists/5
        [HttpPut("{id}")]
        public ActionResult<Artist> UpdateArtist(int id, [FromBody] Artist updatedArtist)
        {
            var artist = _context.Artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            artist.Name = updatedArtist.Name;
            artist.Biography = updatedArtist.Biography;
            artist.Nationality = updatedArtist.Nationality;
            artist.BirthDate = updatedArtist.BirthDate;
            artist.Style = updatedArtist.Style;
            artist.Status = updatedArtist.Status;

            return Ok(artist);
        }

        // PATCH: api/artists/5/status
        [HttpPatch("{id}/status")]
        public ActionResult<Artist> UpdateArtistStatus(int id, [FromBody] StatusUpdate statusUpdate)
        {
            var artist = _context.Artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            if (statusUpdate.Status != "active" && statusUpdate.Status != "inactive")
            {
                return BadRequest(new { message = "סטטוס חייב להיות active או inactive" });
            }

            artist.Status = statusUpdate.Status;

            return Ok(artist);
        }

        // GET: api/artists/5/artworks
        [HttpGet("{id}/artworks")]
        public ActionResult<IEnumerable<Artwork>> GetArtistArtworks(int id)
        {
            var artist = _context.Artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            var artworks = _context.Artworks.Where(a => a.ArtistId == id).ToList();

            return Ok(artworks);
        }
    }

    public class StatusUpdate
    {
        public string Status { get; set; } = string.Empty;
    }
}