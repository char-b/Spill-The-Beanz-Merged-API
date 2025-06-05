using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public CustomersController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: api/Customers <-- this is the endpoint. User clicks link/button
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTOGet>>> GetCustomerList()
        {
            var customers = await _context.Customers.ToListAsync(); //retrieving all rows from DB

            if (customers == null)
            {

                return NotFound();
            }


            var dto = customers.Select(customers => new CustomerDTOGet() //Selects all the rows, goes through each one, and maps each one (object) to the DTO. >= maps. select(parameter name)
            {
                CustomerName = customers.CustomerName,
                Email = customers.Email,
                PhoneNumber = customers.PhoneNumber,
                Address = customers.Address
            }).ToList(); //turns objexts into a new list

            return Ok(dto);


        }

        // GET: api/Customers/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDTOGet>> GetCustomers(int id)
        {
            var customers = await _context.Customers.FindAsync(id);  //we're making 'customers' a type of 'Customers'. 'Customers' is from the DBSet

            if (customers == null)
            {
                return NotFound();
            }

            var dto = new CustomerDTOGet
            {
                CustomerName = customers.CustomerName,
                Email = customers.Email,
                PhoneNumber = customers.PhoneNumber,
                Address = customers.Address
            };


            return Ok(dto);
        }

      
        [HttpPatch("{id}")] //This is used to edit customer details if needed

        public async Task<IActionResult> PatchCustomers(int id, JsonPatchDocument<CustomerDTOP> patchDoc) //what exactly is patchdoc?
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }
            //find an existing customer row
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            //fetching the property values from class model and creating a new DTO from that
            var customerToPatch = new CustomerDTOP
            {
                CustomerName = customer.CustomerName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                PasswordHash = customer.PasswordHash,
                Address = customer.Address
            };

            patchDoc.ApplyTo(customerToPatch);

            if (!TryValidateModel(customerToPatch)) //is this a form of validation? //is patching done here?
            {
                return ValidationProblem(ModelState);
            }
            //updating kinda
            customer.CustomerName = customerToPatch.CustomerName; //so whatever 'patched' value we get, we update?
            customer.Email = customerToPatch.Email;
            customer.PhoneNumber = customerToPatch.PhoneNumber;
            customer.Address = customerToPatch.Address;
            customer.LastVisited = DateTime.UtcNow; //Not in the DTO but, we are now updating the last visted since that's the case? might leave after this? what for? profile visit?

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CustomersExists(id))
                { return NotFound(); }

                else
                {
                    throw;
                }

            }
            return NoContent();
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomers(int id)
        {
            var customers = await _context.Customers.FindAsync(id);
            if (customers == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customers);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CustomersExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerID == id);
        }
    }
}
