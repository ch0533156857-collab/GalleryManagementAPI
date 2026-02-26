using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using restful_code.Models;
using restful_code.Models.Exhibition;

namespace restful_code.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitionsController : ControllerBase
    {
        private readonly IExhibitionService _service;
        private readonly IMapper _mapper;

        public ExhibitionsController(IExhibitionService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExhibitionDTO>>> GetAllExhibitions()
        {
            try
            {
                var exhibitions = await _service.GetAllExhibitionsAsync();
                var exhibitionDTOs = _mapper.Map<List<ExhibitionDTO>>(exhibitions);
                return Ok(exhibitionDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ExhibitionDTO>>> GetActiveExhibitions()
        {
            try
            {
                var exhibitions = await _service.GetActiveExhibitionsAsync();
                var exhibitionDTOs = _mapper.Map<List<ExhibitionDTO>>(exhibitions);
                return Ok(exhibitionDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExhibitionDTO>> GetExhibition(int id)
        {
            try
            {
                var exhibition = await _service.GetExhibitionByIdAsync(id);
                if (exhibition == null)
                {
                    return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
                }
                var exhibitionDTO = _mapper.Map<ExhibitionDTO>(exhibition);
                return Ok(exhibitionDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ExhibitionDTO>> CreateExhibition([FromBody] CreateExhibitionModel model)
        {
            try
            {
                var exhibition = _mapper.Map<Exhibition>(model);
                var created = await _service.CreateExhibitionAsync(exhibition);
                var exhibitionDTO = _mapper.Map<ExhibitionDTO>(created);

                return CreatedAtAction(nameof(GetExhibition), new { id = created.Id }, exhibitionDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ExhibitionDTO>> UpdateExhibition(int id, [FromBody] UpdateExhibitionModel model)
        {
            try
            {
                var updatedExhibition = _mapper.Map<Exhibition>(model);
                var exhibition = await _service.UpdateExhibitionAsync(id, updatedExhibition);
                var exhibitionDTO = _mapper.Map<ExhibitionDTO>(exhibition);

                return Ok(exhibitionDTO);
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

        [HttpPost("{exhibitionId}/artworks/{artworkId}")]
        public async Task<ActionResult<ExhibitionDTO>> AddArtworkToExhibition(int exhibitionId, int artworkId)
        {
            try
            {
                var exhibition = await _service.AddArtworkToExhibitionAsync(exhibitionId, artworkId);
                var exhibitionDTO = _mapper.Map<ExhibitionDTO>(exhibition);
                return Ok(exhibitionDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{exhibitionId}/artworks/{artworkId}")]
        public async Task<ActionResult<ExhibitionDTO>> RemoveArtworkFromExhibition(int exhibitionId, int artworkId)
        {
            try
            {
                var exhibition = await _service.RemoveArtworkFromExhibitionAsync(exhibitionId, artworkId);
                var exhibitionDTO = _mapper.Map<ExhibitionDTO>(exhibition);
                return Ok(exhibitionDTO);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExhibition(int id)
        {
            try
            {
                await _service.DeleteExhibitionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}