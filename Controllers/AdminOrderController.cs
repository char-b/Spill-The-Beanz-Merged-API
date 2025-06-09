using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.Models;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using System.ComponentModel.DataAnnotations;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [ApiController]
    [Route("admin/order")]
    public class AdminOrderController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public AdminOrderController(CoffeeDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .ToListAsync();

            return Ok(orders);
        }

        [HttpPatch("{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto statusDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null) return NotFound();

            order.OrderStatus = statusDto.Status;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
