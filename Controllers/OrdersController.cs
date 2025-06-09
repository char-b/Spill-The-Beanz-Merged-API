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
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public OrdersController(CoffeeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = new Orders
            {
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                OrderStatus = "Pending",
                OrderItems = orderDto.Items.Select(i => new OrderItems
                {
                    ItemId = i.ItemId,
                    Quantity = i.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null) return NotFound();

            return Ok(order);
        }
    }
}