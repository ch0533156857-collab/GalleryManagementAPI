using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restful_code.Models;
using restful_code.Models.Artwork;
using System.Threading.Tasks;

namespace restful_code.Controllers
{
    [Authorize]
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
        public async Task<ActionResult<IEnumerable<ArtworkDTO>>> GetAllArtworks([FromQuery] string? status = null)
        {
            try
            {
                var artworks = await _service.GetAllArtworksAsync(status);
                var artworkDTOs = _mapper.Map<List<ArtworkDTO>>(artworks);
                return Ok(artworkDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ArtworkDTO>> GetArtwork(int id)
        {
            try
            {
                var artwork = await _service.GetArtworkByIdAsync(id);
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
        public async Task<ActionResult<IEnumerable<ArtworkDTO>>> GetArtworksByArtist(int artistId)
        {
            try
            {
                var artworks = await _service.GetArtworksByArtistAsync(artistId);
                var artworkDTOs = _mapper.Map<List<ArtworkDTO>>(artworks);
                return Ok(artworkDTOs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ArtworkDTO>> CreateArtwork([FromBody] CreateArtworkModel model)
        {
            try
            {
                var artwork = _mapper.Map<Artwork>(model);
                var created = await _service.CreateArtworkAsync(artwork);
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
        public async Task<ActionResult<ArtworkDTO>> UpdateArtwork(int id, [FromBody] UpdateArtworkModel model)
        {
            try
            {
                var updatedArtwork = _mapper.Map<Artwork>(model);
                var artwork = await _service.UpdateArtworkAsync(id, updatedArtwork);
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
        public async Task<ActionResult<ArtworkDTO>> UpdateArtworkStatus(int id, [FromBody] StatusUpdate statusUpdate)
        {
            try
            {
                var artwork = await _service.UpdateArtworkStatusAsync(id, statusUpdate.Status);
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
        public async Task<ActionResult> DeleteArtwork(int id)
        {
            try
            {
                await _service.DeleteArtworkAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}