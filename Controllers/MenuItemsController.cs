using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuItemsController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public MenuItemsController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: api/MenuItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuDTOGET>>> GetMenuItems()
        {
            var menuItems = await _context.Menu.ToListAsync();

            if (menuItems == null)
            {
                return NotFound();
            }

            var menuItemsDto = menuItems.Select(menuItems => new MenuDTOGET()
            {
                ItemId = menuItems.ItemId,
                MenuCategory = menuItems.MenuCategory,
                ItemName = menuItems.ItemName,
                ImageUrl = menuItems.ImageUrl,
                Description = menuItems.Description,
                Price = menuItems.Price,
            }).ToList();
       

           return Ok(menuItemsDto);

        }

        //NEED POST


        // DELETE: api/MenuItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenuItems(int id)
        {
            var menuItems = await _context.Menu.FindAsync(id);
            if (menuItems == null)
            {
                return NotFound();
            }

            _context.Menu.Remove(menuItems);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuItemsExists(int id)
        {
            return _context.Menu.Any(e => e.ItemId == id);
        }
    }
}
