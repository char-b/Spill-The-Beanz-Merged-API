        using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public OrdersController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet] //customerOrders
        public async Task<ActionResult<List<CustomerDTOOrderGET>>> GetCustomerOrderList()
        {
            var customerOrders = await _context.Orders
                .Include(order => order.Customer) // Include Customer
                .Include(order => order.OrderItems) // Include OrderItems of the order
                .ThenInclude(oi => oi.Item) // Include related Menu item, if needed
                .Select(order => new CustomerDTOOrderGET
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer.CustomerName,
                    Email = order.Customer.CustomerEmail,
                    PhoneNumber = order.Customer.PhoneNumber,
                    Address = order.Customer.Address,
                    Orders = new List<OrderDto>
                    {
                new OrderDto
                {
                    OrderType = order.OrderType,
                    OrderDate = order.OrderDate,
                    SpecialInstructions = order.SpecialInstructions,
                    OrderStatus = order.OrderStatus,
                    FinalAmount = order.FinalAmount
                }
                    }
                })
                .ToListAsync();

            if (customerOrders == null || !customerOrders.Any())
            {
                return NotFound();
            }

            return Ok(customerOrders);
        }

        //POST Orders

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            //this is to extract from the JWT token
            var customerIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CustomerId");
            if (customerIdClaim == null) return Unauthorized("Customer ID not found in token.");

            int customerId = int.Parse(customerIdClaim.Value);

            var order = new Orders
            {
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow,
                OrderType = orderDto.OrderType,
                TotalAmount = orderDto.TotalAmount,
                TaxAmount = orderDto.TaxAmount,
                FinalAmount = orderDto.FinalAmount,
                OrderStatus = "Received",                                            
                OrderItems = orderDto.Items.Select(i => new OrderItems
                {
                    ItemId = i.ItemId,
                   // UnitPrice = i.UnitPrice, not sure if this is meant to reference item.price
                    Quantity = i.Quantity
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomerOrderById), new { id = order.OrderId }, order);
        }

        // GET: api/Orders/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTOOrderGET>> GetCustomerOrderById(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Item)
                .Where(o => o.OrderId == id)
                .Select(order => new CustomerDTOOrderGET
                {
                    OrderId = order.OrderId,
                    CustomerName = order.Customer.CustomerName,
                    Email = order.Customer.CustomerEmail,
                    PhoneNumber = order.Customer.PhoneNumber,
                    Address = order.Customer.Address,
                    Orders = new List<OrderDto>
                    {
                new OrderDto
                {
                    OrderType = order.OrderType,
                    OrderDate = order.OrderDate,
                    SpecialInstructions = order.SpecialInstructions,
                    OrderStatus = order.OrderStatus,
                    FinalAmount = order.FinalAmount
                }
                    }
                })
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        //post - Junaid for customer creation. customer name, email password

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchOrderStatus(int id, [FromBody] OrderStatusPatchDTO updateStatus)
        {
            if (updateStatus == null || string.IsNullOrWhiteSpace(updateStatus.OrderStatus))
                return BadRequest("OrderStatus is required.");

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            order.OrderStatus = updateStatus.OrderStatus;

            if (!string.IsNullOrWhiteSpace(updateStatus.OrderType))
            {
                order.OrderType = updateStatus.OrderType;
            }

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to update: {ex.Message}");
            }
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrders(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
