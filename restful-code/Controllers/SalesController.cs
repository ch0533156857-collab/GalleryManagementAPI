using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using GalleryManagement.Data.Repositories;
using GalleryManagement.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;

        public SalesController(ISaleService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Sale>> GetAllSales([FromQuery] string? status = null)
        {
            try
            {
                var sales = _service.GetAllSales(status);
                return Ok(sales);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Sale> GetSale(int id)
        {
            try
            {
                var sale = _service.GetSaleById(id);
                if (sale == null)
                {
                    return NotFound(new { message = $"מכירה עם מזהה {id} לא נמצאה" });
                }
                return Ok(sale);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("artwork/{artworkId}")]
        public ActionResult<IEnumerable<Sale>> GetSalesByArtwork(int artworkId)
        {
            try
            {
                var sales = _service.GetSalesByArtwork(artworkId);
                return Ok(sales);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult<Sale> CreateSale([FromBody] Sale sale)
        {
            try
            {
                var created = _service.CreateSale(sale);
                return CreatedAtAction(nameof(GetSale), new { id = created.Id }, created);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Sale> UpdateSale(int id, [FromBody] Sale updatedSale)
        {
            try
            {
                var sale = _service.UpdateSale(id, updatedSale);
                return Ok(sale);
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
        public ActionResult<Sale> UpdateSaleStatus(int id, [FromBody] SaleStatusUpdate statusUpdate)
        {
            try
            {
                var sale = _service.UpdateSaleStatus(id, statusUpdate.Status);
                return Ok(sale);
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
        public ActionResult DeleteSale(int id)
        {
            try
            {
                _service.DeleteSale(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}
