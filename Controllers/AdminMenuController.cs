using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.Models;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("admin/menu")]
    [ApiController]
    public class AdminMenuController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public AdminMenuController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: /admin/menu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenuItems()
        {
            return await _context.Menu.ToListAsync();
        }

        // GET: /admin/menu/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenuItem(int id)
        {
            var item = await _context.Menu.FindAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }

        // POST: /admin/menu
        [HttpPost]
        public async Task<ActionResult<Menu>> CreateMenuItem([FromBody] MenuItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var menuItem = new Menu
            {
                ItemName = dto.ItemName,
                MenuCategory = dto.MenuCategory,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable ?? true,
                IsFeatured = dto.IsFeatured ?? false,
                ImageUrl = dto.ImageUrl,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Menu.Add(menuItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.ItemId }, menuItem);
        }

        // PUT: /admin/menu/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromBody] MenuItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.Menu.FindAsync(id);
            if (item == null)
                return NotFound();

            item.ItemName = dto.ItemName;
            item.MenuCategory = dto.MenuCategory;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.IsAvailable = dto.IsAvailable ?? item.IsAvailable;
            item.IsFeatured = dto.IsFeatured ?? item.IsFeatured;
            item.ImageUrl = dto.ImageUrl;
            item.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: /admin/menu/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItem(int id)
        {
            var item = await _context.Menu.FindAsync(id);
            if (item == null)
                return NotFound();

            _context.Menu.Remove(item);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
