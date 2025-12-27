using Microsoft.AspNetCore.Mvc;
using restful_code.Data;
using restful_code.Entities;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitionsController : ControllerBase
    {
        private readonly DataContext _context;

        public ExhibitionsController(DataContext context)
        {
            _context = context;
        }

        // GET: api/exhibitions
        [HttpGet]
        public ActionResult<IEnumerable<Exhibition>> GetAllExhibitions([FromQuery] bool? active = null)
        {
            var exhibitions = _context.Exhibitions.AsQueryable();

            if (active.HasValue && active.Value)
            {
                var today = DateTime.Now;
                exhibitions = exhibitions.Where(e => e.StartDate <= today && e.EndDate >= today);
            }

            return Ok(exhibitions.ToList());
        }

        // GET: api/exhibitions/5
        [HttpGet("{id}")]
        public ActionResult<Exhibition> GetExhibition(int id)
        {
            var exhibition = _context.Exhibitions.FirstOrDefault(e => e.Id == id);

            if (exhibition == null)
            {
                return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
            }

            return Ok(exhibition);
        }

        // POST: api/exhibitions
        [HttpPost]
        public ActionResult<Exhibition> CreateExhibition([FromBody] Exhibition exhibition)
        {
            exhibition.Id = _context.NextExhibitionId++;
            exhibition.CreatedAt = DateTime.Now;
            exhibition.ArtworkIds = new List<int>();

            _context.Exhibitions.Add(exhibition);

            return CreatedAtAction(nameof(GetExhibition), new { id = exhibition.Id }, exhibition);
        }

        // PUT: api/exhibitions/5
        [HttpPut("{id}")]
        public ActionResult<Exhibition> UpdateExhibition(int id, [FromBody] Exhibition updatedExhibition)
        {
            var exhibition = _context.Exhibitions.FirstOrDefault(e => e.Id == id);

            if (exhibition == null)
            {
                return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
            }

            exhibition.Name = updatedExhibition.Name;
            exhibition.Description = updatedExhibition.Description;
            exhibition.StartDate = updatedExhibition.StartDate;
            exhibition.EndDate = updatedExhibition.EndDate;
            exhibition.Location = updatedExhibition.Location;
            exhibition.CuratorName = updatedExhibition.CuratorName;

            return Ok(exhibition);
        }

        // DELETE: api/exhibitions/5
        [HttpDelete("{id}")]
        public ActionResult DeleteExhibition(int id)
        {
            var exhibition = _context.Exhibitions.FirstOrDefault(e => e.Id == id);

            if (exhibition == null)
            {
                return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
            }

            _context.Exhibitions.Remove(exhibition);

            return Ok(new { message = "התערוכה נמחקה בהצלחה", deletedId = id });
        }

        // GET: api/exhibitions/5/artworks
        [HttpGet("{id}/artworks")]
        public ActionResult<IEnumerable<Artwork>> GetExhibitionArtworks(int id)
        {
            var exhibition = _context.Exhibitions.FirstOrDefault(e => e.Id == id);

            if (exhibition == null)
            {
                return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
            }

            var artworks = _context.Artworks
                .Where(a => exhibition.ArtworkIds.Contains(a.Id))
                .ToList();

            return Ok(artworks);
        }

        // POST: api/exhibitions/5/artworks
        [HttpPost("{id}/artworks")]
        public ActionResult AddArtworkToExhibition(int id, [FromBody] ArtworkToExhibition request)
        {
            var exhibition = _context.Exhibitions.FirstOrDefault(e => e.Id == id);

            if (exhibition == null)
            {
                return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
            }

            if (exhibition.ArtworkIds.Contains(request.ArtworkId))
            {
                return BadRequest(new { message = "היצירה כבר נמצאת בתערוכה" });
            }

            exhibition.ArtworkIds.Add(request.ArtworkId);

            return Ok(new { message = "היצירה נוספה לתערוכה בהצלחה", exhibition });
        }

        // DELETE: api/exhibitions/5/artworks/3
        [HttpDelete("{id}/artworks/{artworkId}")]
        public ActionResult RemoveArtworkFromExhibition(int id, int artworkId)
        {
            var exhibition = _context.Exhibitions.FirstOrDefault(e => e.Id == id);

            if (exhibition == null)
            {
                return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
            }

            if (!exhibition.ArtworkIds.Contains(artworkId))
            {
                return NotFound(new { message = $"יצירה עם מזהה {artworkId} לא נמצאת בתערוכה" });
            }

            exhibition.ArtworkIds.Remove(artworkId);

            return Ok(new { message = "היצירה הוסרה מהתערוכה בהצלחה", exhibition });
        }
    }

    public class ArtworkToExhibition
    {
        public int ArtworkId { get; set; }
    }
}