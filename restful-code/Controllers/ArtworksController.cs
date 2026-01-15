using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using restful_code.Models;
using restful_code.Models.Artwork;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtworksController : ControllerBase
    {
        private readonly IArtworkService _service;
        private readonly IMapper _mapper;

        public ArtworksController(IArtworkService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtworkDTO>> GetAllArtworks([FromQuery] string? status = null)
        {
            try
            {
                var artworks = _service.GetAllArtworks(status);
                var artworkDTOs = _mapper.Map<List<ArtworkDTO>>(artworks);
                return Ok(artworkDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ArtworkDTO> GetArtwork(int id)
        {
            try
            {
                var artwork = _service.GetArtworkById(id);
                if (artwork == null)
                {
                    return NotFound(new { message = $"יצירה עם מזהה {id} לא נמצאה" });
                }
                var artworkDTO = _mapper.Map<ArtworkDTO>(artwork);
                return Ok(artworkDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("artist/{artistId}")]
        public ActionResult<IEnumerable<ArtworkDTO>> GetArtworksByArtist(int artistId)
        {
            try
            {
                var artworks = _service.GetArtworksByArtist(artistId);
                var artworkDTOs = _mapper.Map<List<ArtworkDTO>>(artworks);
                return Ok(artworkDTOs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<ArtworkDTO> CreateArtwork([FromBody] CreateArtworkModel model)
        {
            try
            {
                // 🎯 נקי ופשוט
                var artwork = _mapper.Map<Artwork>(model);
                var created = _service.CreateArtwork(artwork);
                var artworkDTO = _mapper.Map<ArtworkDTO>(created);

                return CreatedAtAction(nameof(GetArtwork), new { id = created.Id }, artworkDTO);
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
        public ActionResult<ArtworkDTO> UpdateArtwork(int id, [FromBody] UpdateArtworkModel model)
        {
            try
            {
                // 🎯 נקי ופשוט
                var updatedArtwork = _mapper.Map<Artwork>(model);
                var artwork = _service.UpdateArtwork(id, updatedArtwork);
                var artworkDTO = _mapper.Map<ArtworkDTO>(artwork);

                return Ok(artworkDTO);
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
        public ActionResult<ArtworkDTO> UpdateArtworkStatus(int id, [FromBody] StatusUpdate statusUpdate)
        {
            try
            {
                var artwork = _service.UpdateArtworkStatus(id, statusUpdate.Status);
                var artworkDTO = _mapper.Map<ArtworkDTO>(artwork);
                return Ok(artworkDTO);
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