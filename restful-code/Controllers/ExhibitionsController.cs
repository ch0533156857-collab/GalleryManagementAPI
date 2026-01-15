using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using restful_code.Models;
using restful_code.Models.Exhibition;

namespace restful_code.Controllers
{
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
        public ActionResult<IEnumerable<ExhibitionDTO>> GetAllExhibitions()
        {
            try
            {
                var exhibitions = _service.GetAllExhibitions();
                var exhibitionDTOs = _mapper.Map<List<ExhibitionDTO>>(exhibitions);
                return Ok(exhibitionDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        public ActionResult<IEnumerable<ExhibitionDTO>> GetActiveExhibitions()
        {
            try
            {
                var exhibitions = _service.GetActiveExhibitions();
                var exhibitionDTOs = _mapper.Map<List<ExhibitionDTO>>(exhibitions);
                return Ok(exhibitionDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<ExhibitionDTO> GetExhibition(int id)
        {
            try
            {
                var exhibition = _service.GetExhibitionById(id);
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
        public ActionResult<ExhibitionDTO> CreateExhibition([FromBody] CreateExhibitionModel model)
        {
            try
            {
                // 🎯 נקי ופשוט
                var exhibition = _mapper.Map<Exhibition>(model);
                var created = _service.CreateExhibition(exhibition);
                var exhibitionDTO = _mapper.Map<ExhibitionDTO>(created);

                return CreatedAtAction(nameof(GetExhibition), new { id = created.Id }, exhibitionDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<ExhibitionDTO> UpdateExhibition(int id, [FromBody] UpdateExhibitionModel model)
        {
            try
            {
                // 🎯 נקי ופשוט
                var updatedExhibition = _mapper.Map<Exhibition>(model);
                var exhibition = _service.UpdateExhibition(id, updatedExhibition);
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
        public ActionResult<ExhibitionDTO> AddArtworkToExhibition(int exhibitionId, int artworkId)
        {
            try
            {
                var exhibition = _service.AddArtworkToExhibition(exhibitionId, artworkId);
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
        public ActionResult<ExhibitionDTO> RemoveArtworkFromExhibition(int exhibitionId, int artworkId)
        {
            try
            {
                var exhibition = _service.RemoveArtworkFromExhibition(exhibitionId, artworkId);
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
        public ActionResult DeleteExhibition(int id)
        {
            try
            {
                _service.DeleteExhibition(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}