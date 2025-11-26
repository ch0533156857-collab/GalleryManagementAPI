using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using restful_code.Entities;


namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        // רשימה סטטית שמחליפה את הדאטאבייס בשלב זה
        private static List<Artist> _artists = new List<Artist>
        {
            new Artist
            {
                Id = 1,
                Name = "לאונרדו דה וינצ'י",
                Biography = "אמן רנסנס איטלקי",
                Nationality = "איטליה",
                BirthDate = new DateTime(1452, 4, 15),
                Style = "רנסנס",
                Status = "active"
            },
            new Artist
            {
                Id = 2,
                Name = "פבלו פיקאסו",
                Biography = "אמן קוביסטי ספרדי",
                Nationality = "ספרד",
                BirthDate = new DateTime(1881, 10, 25),
                Style = "קוביזם",
                Status = "active"
            }
        };

        private static int _nextId = 3;

        // GET: api/artists
        [HttpGet]
        public ActionResult<IEnumerable<Artist>> GetAllArtists([FromQuery] string? status = null)
        {
            var artists = _artists.AsQueryable();

            // סינון לפי סטטוס אם הוגדר
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
            var artist = _artists.FirstOrDefault(a => a.Id == id);

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
            artist.Id = _nextId++;
            artist.CreatedAt = DateTime.Now;
            artist.Status = "active"; // ברירת מחדל

            _artists.Add(artist);

            return CreatedAtAction(nameof(GetArtist), new { id = artist.Id }, artist);
        }

        // PUT: api/artists/5
        [HttpPut("{id}")]
        public ActionResult<Artist> UpdateArtist(int id, [FromBody] Artist updatedArtist)
        {
            var artist = _artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            // עדכון כל השדות
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
            var artist = _artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            // בדיקה שהסטטוס תקין
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
            var artist = _artists.FirstOrDefault(a => a.Id == id);

            if (artist == null)
            {
                return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
            }

            // נשלוף יצירות מה-Controller של Artworks (נממש זאת בהמשך)
            var artworks = ArtworksController.GetArtworksByArtistId(id);

            return Ok(artworks);
        }
    }

    // מחלקת עזר לעדכון סטטוס
    public class StatusUpdate
    {
        public string Status { get; set; } = string.Empty;
    }
}