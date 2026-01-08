using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        private readonly IArtworkService _service;

        public ArtworksController(IArtworkService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Artwork>> GetAllArtworks([FromQuery] string? status = null)
        {
            try
            {
                var artworks = _service.GetAllArtworks(status);
                return Ok(artworks);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Artwork> GetArtwork(int id)
        {
            try
            {
                var artwork = _service.GetArtworkById(id);
                if (artwork == null)
                {
                    return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
                }
                return Ok(artwork);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("artist/{artistId}")]
        public ActionResult<IEnumerable<Artwork>> GetArtworksByArtist(int artistId)
        {
            try
            {
                var artworks = _service.GetArtworksByArtist(artistId);
                return Ok(artworks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Artwork> CreateArtwork([FromBody] Artwork artwork)
        {
            try
            {
                var created = _service.CreateArtwork(artwork);
                return CreatedAtAction(nameof(GetArtwork), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Artwork> UpdateArtwork(int id, [FromBody] Artwork updatedArtwork)
        {
            try
            {
                var artwork = _service.UpdateArtwork(id, updatedArtwork);
                return Ok(artwork);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPatch("{id}/status")]
        public ActionResult<Artwork> UpdateArtworkStatus(int id, [FromBody] StatusUpdate statusUpdate)
        {
            try
            {
                var artwork = _service.UpdateArtworkStatus(id, statusUpdate.Status);
                return Ok(artwork);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteArtwork(int id)
        {
            try
            {
                _service.DeleteArtwork(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}