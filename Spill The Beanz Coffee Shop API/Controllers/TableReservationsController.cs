using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;
using Spill_The_Beanz_Coffee_Shop_API.Models;
using static NuGet.Packaging.PackagingConstants;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableReservationsController : ControllerBase
    {
        private readonly CoffeeDbContext _context;

        public TableReservationsController(CoffeeDbContext context)
        {
            _context = context;
        }

        // GET: api/TableReservations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTORes>>> GetTableReservations()
        {
            var customersRes = await _context.TableReservations //using the Customers as the base
            .Include(customer => customer.Customers) //use the tableres property in there
          //.ThenInclude(customerRes => customerRes.Customers)
            .Select(customerRes => new CustomerDTORes()
            { //create new DTO object of customer. so only certain data is seen. 
                ReservationId = customerRes.ReservationId,
                CustomerName = customerRes.Customers.CustomerName,
                Email = customerRes.Customers.Email,
                PhoneNumber = customerRes.Customers.CustomerName,
                TableReservations = new List<TableResDTO>
                { new TableResDTO
                {
                    TableId = customerRes.TableId,
                    ReservationDate = customerRes.ReservationDate,
                    StartTime = customerRes.StartTime,
                    EndTime = customerRes.EndTime,
                    PartySize = customerRes.PartySize,
                    SpecialRequests = customerRes.SpecialRequests,
                    ReservationStatus = customerRes.ReservationStatus
                }
                }                                 
            }).ToListAsync();

            return Ok(customersRes);
        }

    

        [HttpPatch("{id}")]
        public async Task<ActionResult<TableReservations>>PatchTableReservationStatus(int id, [FromBody]TableResDTO updateStatus) //you patch the DTO but you're returning back to the model
        {
            if (updateStatus == null)
            {
                return BadRequest();
            }

            var customersRes = await _context.TableReservations.FindAsync(id); //find all the customers in the list

            if (customersRes == null)
            {
                return NotFound();
            }
            //do i only 'copy' the one value i want to change?

            var reservationStatus = new TableResDTO //now we assign the current value from the MODEL to the value of the DTO
            {
                ReservationStatus = updateStatus.ReservationStatus
            };

           // updateStatus.ApplyTo(reservationStatus);

              if (!TryValidateModel(reservationStatus)) //is this a form of validation? //is patching done here?
              {
                  return ValidationProblem(ModelState);
              }

               customersRes.ReservationStatus = reservationStatus.ReservationStatus;

                try
                {
              await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TableReservationsExists(id))
                             { return NotFound(); 
                }
                   else
                   {
                       throw;
                   }
              }
              return NoContent();
        }

        

        // DELETE: api/TableReservations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTableReservations(int id)
        {
            var tableReservations = await _context.TableReservations.FindAsync(id);
            if (tableReservations == null)
            {
                return NotFound();
            }

            _context.TableReservations.Remove(tableReservations);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TableReservationsExists(int id)
        {
            return _context.TableReservations.Any(e => e.ReservationId == id);
        }
    }
}
