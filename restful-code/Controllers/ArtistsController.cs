using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using restful_code.Models;
using restful_code.Models.Artist;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        private readonly IArtistService _service;
        private readonly IMapper _mapper;

        public ArtistsController(IArtistService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ArtistDTO>> GetAllArtists([FromQuery] string? status = null)
        {
            try
            {
                var artists = _service.GetAllArtists(status);
                var artistDTOs = _mapper.Map<List<ArtistDTO>>(artists);
                return Ok(artistDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ArtistDTO> GetArtist(int id)
        {
            try
            {
                var artist = _service.GetArtistById(id);
                if (artist == null)
                {
                    return NotFound(new { message = $"אמן עם מזהה {id} לא נמצא" });
                }
                var artistDTO = _mapper.Map<ArtistDTO>(artist);
                return Ok(artistDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<ArtistDTO> CreateArtist([FromBody] CreateArtistModel model)
        {
            try
            {
                // 🎯 פשוט ונקי - AutoMapper עושה הכל!
                var artist = _mapper.Map<Artist>(model);
                var created = _service.CreateArtist(artist);
                var artistDTO = _mapper.Map<ArtistDTO>(created);

                return CreatedAtAction(nameof(GetArtist), new { id = created.Id }, artistDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ArtistDTO> UpdateArtist(int id, [FromBody] UpdateArtistModel model)
        {
            try
            {
                // 🎯 פשוט ונקי
                var updatedArtist = _mapper.Map<Artist>(model);
                var artist = _service.UpdateArtist(id, updatedArtist);
                var artistDTO = _mapper.Map<ArtistDTO>(artist);

                return Ok(artistDTO);
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
        public ActionResult<ArtistDTO> UpdateArtistStatus(int id, [FromBody] StatusUpdate statusUpdate)
        {
            try
            {
                var artist = _service.UpdateArtistStatus(id, statusUpdate.Status);
                var artistDTO = _mapper.Map<ArtistDTO>(artist);
                return Ok(artistDTO);
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
        public ActionResult<IEnumerable<ArtworkDTO>> GetArtistArtworks(int id)
        {
            try
            {
                var artworks = _service.GetArtistArtworks(id);
                var artworkDTOs = _mapper.Map<List<ArtworkDTO>>(artworks);
                return Ok(artworkDTOs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}