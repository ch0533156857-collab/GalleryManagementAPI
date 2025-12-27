using Microsoft.AspNetCore.Mvc;
using restful_code.Data;
using restful_code.Entities;

namespace restful_code.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly DataContext _context;

        public SalesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/sales
        [HttpGet]
        public ActionResult<IEnumerable<Sale>> GetAllSales(
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var sales = _context.Sales.AsQueryable();

            if (from.HasValue)
            {
                sales = sales.Where(s => s.SaleDate >= from.Value);
            }

            if (to.HasValue)
            {
                sales = sales.Where(s => s.SaleDate <= to.Value);
            }

            return Ok(sales.ToList());
        }

        // GET: api/sales/5
        [HttpGet("{id}")]
        public ActionResult<Sale> GetSale(int id)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == id);

            if (sale == null)
            {
                return NotFound(new { message = $"מכירה עם מזהה {id} לא נמצאה" });
            }

            return Ok(sale);
        }

        // POST: api/sales
        [HttpPost]
        public ActionResult<Sale> CreateSale([FromBody] Sale sale)
        {
            sale.Id = _context.NextSaleId++;
            sale.SaleDate = DateTime.Now;
            sale.Status = "completed";

            _context.Sales.Add(sale);

            return CreatedAtAction(nameof(GetSale), new { id = sale.Id }, sale);
        }

        // PUT: api/sales/5
        [HttpPut("{id}")]
        public ActionResult<Sale> UpdateSale(int id, [FromBody] Sale updatedSale)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == id);

            if (sale == null)
            {
                return NotFound(new { message = $"מכירה עם מזהה {id} לא נמצאה" });
            }

            sale.BuyerName = updatedSale.BuyerName;
            sale.BuyerEmail = updatedSale.BuyerEmail;
            sale.SalePrice = updatedSale.SalePrice;
            sale.PaymentMethod = updatedSale.PaymentMethod;
            sale.Status = updatedSale.Status;
            sale.Notes = updatedSale.Notes;

            return Ok(sale);
        }

        // DELETE: api/sales/5
        [HttpDelete("{id}")]
        public ActionResult DeleteSale(int id)
        {
            var sale = _context.Sales.FirstOrDefault(s => s.Id == id);

            if (sale == null)
            {
                return NotFound(new { message = $"מכירה עם מזהה {id} לא נמצאה" });
            }

            _context.Sales.Remove(sale);

            return Ok(new { message = "המכירה בוטלה בהצלחה", cancelledId = id });
        }

        // GET: api/sales/stats
        [HttpGet("stats")]
        public ActionResult GetSalesStatistics()
        {
            var completedSales = _context.Sales.Where(s => s.Status == "completed").ToList();

            var stats = new
            {
                totalSales = completedSales.Count,
                totalRevenue = completedSales.Sum(s => s.SalePrice),
                averageSalePrice = completedSales.Any() ? completedSales.Average(s => s.SalePrice) : 0,
                highestSale = completedSales.Any() ? completedSales.Max(s => s.SalePrice) : 0,
                lowestSale = completedSales.Any() ? completedSales.Min(s => s.SalePrice) : 0,
                salesByMonth = completedSales
                    .GroupBy(s => new { s.SaleDate.Year, s.SaleDate.Month })
                    .Select(g => new
                    {
                        year = g.Key.Year,
                        month = g.Key.Month,
                        count = g.Count(),
                        revenue = g.Sum(s => s.SalePrice)
                    })
                    .OrderBy(x => x.year)
                    .ThenBy(x => x.month)
                    .ToList()
            };

            return Ok(stats);
        }
    }
}