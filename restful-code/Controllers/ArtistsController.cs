using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _service;

        public ArtistsController(IArtistService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Artist>> GetAllArtists([FromQuery] string? status = null)
        {
            try
            {
                var artists = _service.GetAllArtists(status);
                return Ok(artists);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Artist> GetArtist(int id)
        {
            try
            {
                var artist = _service.GetArtistById(id);
                if (artist == null)
                {
                    return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
                }
                return Ok(artist);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Artist> CreateArtist([FromBody] Artist artist)
        {
            try
            {
                var created = _service.CreateArtist(artist);
                return CreatedAtAction(nameof(GetArtist), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Artist> UpdateArtist(int id, [FromBody] Artist updatedArtist)
        {
            try
            {
                var artist = _service.UpdateArtist(id, updatedArtist);
                return Ok(artist);
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
        public ActionResult<Artist> UpdateArtistStatus(int id, [FromBody] StatusUpdate statusUpdate)
        {
            try
            {
                var artist = _service.UpdateArtistStatus(id, statusUpdate.Status);
                return Ok(artist);
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

        [HttpGet("{id}/artworks")]
        public ActionResult<IEnumerable<Artwork>> GetArtistArtworks(int id)
        {
            try
            {
                var artworks = _service.GetArtistArtworks(id);
                return Ok(artworks);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}