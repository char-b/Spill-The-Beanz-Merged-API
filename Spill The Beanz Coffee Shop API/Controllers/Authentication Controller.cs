﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.Models;
using Spill_The_Beanz_Coffee_Shop_API.DTOs;

namespace Spill_The_Beanz_Coffee_Shop_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly CoffeeDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(CoffeeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerDto model)
        {

            if (await _context.Customers.AnyAsync(c => c.CustomerEmail == model.CustomerEmail))
                return BadRequest("Email is already registered.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.Password); //password hasing

            var customer = new Customers
            {
                CustomerName = model.CustomerName,
                CustomerEmail = model.CustomerEmail,
                PhoneNumber = model.PhoneNumber,
                PasswordHash = hashedPassword,
                Address = model.Address,
                CreatedAt = DateTime.Now,
                IsActive = true,
                LoyaltyPoints = 0
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        //LOGIN CONTROLLERS

        [HttpPost("admin/login")]
        public async Task<IActionResult> AdminLogin([FromBody] LoginDto model)
        {
            var admin = await _context.Admin.SingleOrDefaultAsync(a => a.Email == model.Email);

            if (admin == null)
                return Unauthorized("Invalid email or password.");

            if (string.IsNullOrEmpty(admin.Password))
                return Unauthorized("Invalid email or password.");

            bool validPassword;
            try
            {
                validPassword = BCrypt.Net.BCrypt.Verify(model.Password, admin.Password);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                // Hash in database is invalid format
                return Unauthorized("Invalid email or password.");
            }

            if (!validPassword)
                return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(admin.AdminId, admin.Email, "Admin", admin.adminName);

            return Ok(new
            {
                token,
                adminId = admin.AdminId,
                name = admin.adminName
            });
        } //NOT HASHED IN DB

        [HttpPost("customer/login")]
        public async Task<IActionResult> CustomerLogin([FromBody] LoginDto model)
        {
            var customer = await _context.Customers.SingleOrDefaultAsync(c => c.CustomerEmail == model.Email);

            if (customer == null || !BCrypt.Net.BCrypt.Verify(model.Password, customer.PasswordHash))
                return Unauthorized("Invalid email or password.");

            var token = GenerateJwtToken(customer.CustomerID, customer.CustomerEmail, "Customer", customer.CustomerName);

            return Ok(new
            {
                token,
                //customerId = customer.CustomerID,
                customerName = customer.CustomerName
            });
        }

        private string GenerateJwtToken(int userId, string email, string role, string customerName)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                            new Claim("CustomerId", userId.ToString()),
                            new Claim(ClaimTypes.Email, email),
                            new Claim(ClaimTypes.Role, role),
                            new Claim(ClaimTypes.Name, customerName)
                        }),
                    Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpiresInMinutes"]!)),
                    Issuer = jwtSettings["Issuer"],
                    Audience = jwtSettings["Audience"],
                    SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
