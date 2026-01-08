using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExhibitionsController : ControllerBase
    {
        private readonly IExhibitionService _service;

        public ExhibitionsController(IExhibitionService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Exhibition>> GetAllExhibitions()
        {
            try
            {
                var exhibitions = _service.GetAllExhibitions();
                return Ok(exhibitions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("active")]
        public ActionResult<IEnumerable<Exhibition>> GetActiveExhibitions()
        {
            try
            {
                var exhibitions = _service.GetActiveExhibitions();
                return Ok(exhibitions);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Exhibition> GetExhibition(int id)
        {
            try
            {
                var exhibition = _service.GetExhibitionById(id);
                if (exhibition == null)
                {
                    return NotFound(new { message = $"תערוכה עם מזהה {id} לא נמצאה" });
                }
                return Ok(exhibition);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Exhibition> CreateExhibition([FromBody] Exhibition exhibition)
        {
            try
            {
                var created = _service.CreateExhibition(exhibition);
                return CreatedAtAction(nameof(GetExhibition), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Exhibition> UpdateExhibition(int id, [FromBody] Exhibition updatedExhibition)
        {
            try
            {
                var exhibition = _service.UpdateExhibition(id, updatedExhibition);
                return Ok(exhibition);
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
        public ActionResult<Exhibition> AddArtworkToExhibition(int exhibitionId, int artworkId)
        {
            try
            {
                var exhibition = _service.AddArtworkToExhibition(exhibitionId, artworkId);
                return Ok(exhibition);
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
        public ActionResult<Exhibition> RemoveArtworkFromExhibition(int exhibitionId, int artworkId)
        {
            try
            {
                var exhibition = _service.RemoveArtworkFromExhibition(exhibitionId, artworkId);
                return Ok(exhibition);
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