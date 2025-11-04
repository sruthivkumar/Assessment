using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductOrderAPI.Data;
using ProductOrderAPI.DTOs.Prouct;
using ProductOrderAPI.Helpers;
using ProductOrderAPI.Model;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ProductOrderAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProductsController(AppDbContext db)
        {
            _db = db;
        }



        [HttpGet]
        public async Task<ActionResult<PagedResult<ProductListDto>>>
        Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery]
string? search = null, [FromQuery] string? sort = null)

        {
            var query = _db.Products.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(p => p.Name.Contains(search));
            // sorting: name, -name, price, -price
            if (!string.IsNullOrWhiteSpace(sort))
            {
                switch (sort.ToLower())
                {
                    case "name": query = query.OrderBy(p => p.Name); break;
                    case "-name":
                        query = query.OrderByDescending(p => p.Name);
                        break;
                    case "price": query = query.OrderBy(p => p.Price); break;
                    case "-price":
                        query = query.OrderByDescending(p => p.Price);
                        break;
                    default: break;
                }
            }
            else query = query.OrderBy(p => p.Id);
            var total = await query.CountAsync();
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price =
            p.Price,
                Stock = p.Stock
            })
            .ToListAsync();
            var res = new PagedResult<ProductListDto>
            {
                Page = page,
                PageSize =
            pageSize,
                Total = total,
                Items = items
            };
            return Ok(res);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            return Ok(new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price =
            p.Price,
                Stock = p.Stock
            });
        }
       

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (await _db.Products.AnyAsync(x => x.Name == dto.Name))

                return BadRequest(new { message = "Product name must be unique" });
            var p = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Stock =
            dto.Stock
            };
            _db.Products.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }
      


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto
        dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            if (p.Name != dto.Name && await _db.Products.AnyAsync(x => x.Name ==
            dto.Name))
                return BadRequest(new { message = "Product name must be unique" });
            p.Name = dto.Name; p.Price = dto.Price; p.Stock = dto.Stock;
            await _db.SaveChangesAsync();
            return NoContent();
        }
    


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Products.FindAsync(id);
            if (p == null) return NotFound();
            _db.Products.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}