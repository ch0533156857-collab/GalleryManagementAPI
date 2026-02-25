using AutoMapper;
using GalleryManagement.Core.DTOs;
using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using restful_code.Models;
using restful_code.Models.Sale;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _service;
        private readonly IMapper _mapper;

        public SalesController(ISaleService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleDTO>>> GetAllSales([FromQuery] string? status = null)
        {
            try
            {
                var sales = await _service.GetAllSalesAsync(status);
                var saleDTOs = _mapper.Map<List<SaleDTO>>(sales);
                return Ok(saleDTOs);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SaleDTO>> GetSale(int id)
        {
            try
            {
                var sale = await _service.GetSaleByIdAsync(id);
                if (sale == null)
                {
                    return NotFound(new { message = $"מכירה עם מזהה {id} לא נמצאה" });
                }
                var saleDTO = _mapper.Map<SaleDTO>(sale);
                return Ok(saleDTO);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("artwork/{artworkId}")]
        public async Task<ActionResult<IEnumerable<SaleDTO>>> GetSalesByArtwork(int artworkId)
        {
            try
            {
                var sales = await _service.GetSalesByArtworkAsync(artworkId);
                var saleDTOs = _mapper.Map<List<SaleDTO>>(sales);
                return Ok(saleDTOs);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SaleDTO>> CreateSale([FromBody] CreateSaleModel model)
        {
            try
            {
                var sale = _mapper.Map<Sale>(model);
                var created = await _service.CreateSaleAsync(sale);
                var saleDTO = _mapper.Map<SaleDTO>(created);

                return CreatedAtAction(nameof(GetSale), new { id = created.Id }, saleDTO);
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
        public async Task<ActionResult<SaleDTO>> UpdateSale(int id, [FromBody] UpdateSaleModel model)
        {
            try
            {
                var updatedSale = _mapper.Map<Sale>(model);
                var sale = await _service.UpdateSaleAsync(id, updatedSale);
                var saleDTO = _mapper.Map<SaleDTO>(sale);

                return Ok(saleDTO);
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
        public async Task<ActionResult<SaleDTO>> UpdateSaleStatus(int id, [FromBody] SaleStatusUpdate statusUpdate)
        {
            try
            {
                var sale = await _service.UpdateSaleStatusAsync(id, statusUpdate.Status);
                var saleDTO = _mapper.Map<SaleDTO>(sale);
                return Ok(saleDTO);
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
        public async Task<ActionResult> DeleteSale(int id)
        {
            try
            {
                await _service.DeleteSaleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}