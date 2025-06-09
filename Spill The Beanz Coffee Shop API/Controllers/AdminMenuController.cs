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

        //GET: /admin/menu //COPIED THIS FROM MY MENU ITEMS CONTROLLER TO MATCH - Char
       [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenuItems()
        {
            return await _context.Menu.ToListAsync();
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MenuDTOGET>>> GetMenuItems()
        //{
        //    var menuItems = await _context.Menu.ToListAsync();

        //    if (menuItems == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuItemsDto = menuItems.Select(menuItems => new MenuDTOGET()
        //    {
        //        ItemId = menuItems.ItemId,
        //        ImageUrl = menuItems.ImageUrl,
        //        ItemName = menuItems.ItemName,
        //        Category = menuItems.Category,                     
        //        Description = menuItems.Description,
        //        Price = menuItems.Price,
        //    }).ToList();


        //    return Ok(menuItemsDto);

        //}




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
        //[HttpPost]
        //[Consumes("multipart/form-data")]
        //public async Task<ActionResult<Menu>> CreateMenuItem([FromForm] MenuItemDto dto, [FromForm] IFormFile? imageFile)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    string imagePath = null;

        //    if (imageFile != null && imageFile.Length > 0)
        //    {
        //        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Image Uploads");
        //        Directory.CreateDirectory(uploadsFolder);

        //        var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
        //        var filePath = Path.Combine(uploadsFolder, fileName);

        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(stream);
        //        }

        //        imagePath = $"/Image Uploads/{fileName}";
        //    }

        //    var menuItem = new Menu
        //    {
        //        ItemName = dto.ItemName,
        //        Category = dto.Category,
        //        Description = dto.Description,
        //        Price = dto.Price,
        //        IsAvailable = dto.IsAvailable ?? true,
        //        IsFeatured = dto.IsFeatured ?? false,
        //        ImageUrl = imagePath,
        //        CreatedAt = DateTime.UtcNow,
        //        UpdatedAt = DateTime.UtcNow
        //    };

        //    _context.Menu.Add(menuItem);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction(nameof(GetMenuItem), new { id = menuItem.ItemId }, menuItem);
        //}



        //  PUT: /admin/menu/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMenuItem(int id, [FromForm] MenuItemDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var item = await _context.Menu.FindAsync(id);
            if (item == null)
                return NotFound();

            item.ItemName = dto.ItemName;
            item.Category = dto.Category;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.IsAvailable = dto.IsAvailable ?? item.IsAvailable;
            item.IsFeatured = dto.IsFeatured ?? item.IsFeatured;
           // item.ImageUrl = dto.ImageUrl;
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
