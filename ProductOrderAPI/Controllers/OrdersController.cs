using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductOrderAPI.Data;
using ProductOrderAPI.DTOs.Order;
using ProductOrderAPI.Model;

namespace ProductOrderAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db)
        {
            _db = db;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _db.Orders.Include(o => o.Product).Select(o => new
            OrderDto
            {
                Id = o.Id,
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Qty = o.Qty,
                Total = o.Total,
                OrderDate = o.OrderDate
            }).ToListAsync();
            return Ok(orders);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var o = await _db.Orders.Include(x => x.Product).SingleOrDefaultAsync(x
            => x.Id == id);
            if (o == null) return NotFound();
            return Ok(new OrderDto
            {
                Id = o.Id,
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Qty = o.Qty,
                Total = o.Total,
                OrderDate =
            o.OrderDate
            });
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateDto dto)
        {
          
        if (!ModelState.IsValid) return BadRequest(ModelState);
            // Use a transaction to ensure atomicity
            using var tx = await _db.Database.BeginTransactionAsync();
            try
            {
                var product = await _db.Products.SingleOrDefaultAsync(p => p.Id ==
                dto.ProductId);
                if (product == null)
                {
                    return BadRequest(new { message = "Product does not exist" });
                }
                if (product.Stock < dto.Qty)
                {
                    return BadRequest(new { message = "Insufficient stock" });
                }
                product.Stock -= dto.Qty;
                var order = new Order
                {
                    ProductId = product.Id,
                    Qty = dto.Qty,
                    Total = product.Price * dto.Qty
                };
                _db.Orders.Add(order);
                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return CreatedAtAction(nameof(Get), new { id = order.Id }, new
                OrderDto
                {
                    Id = order.Id,
                    ProductId = order.ProductId,
                    ProductName = product.Name,
                    Qty = order.Qty,
                    Total = order.Total,
                    OrderDate = order.OrderDate
                });
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

[HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            // Optionally: return stock
            var product = await _db.Products.FindAsync(order.ProductId);
            if (product != null)
            {
                product.Stock += order.Qty;
            }
            _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }

}
