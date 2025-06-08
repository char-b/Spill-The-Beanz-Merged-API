using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Spill_The_Beanz_Coffee_Shop_API.DB_Context;
using Spill_The_Beanz_Coffee_Shop_API.DTOs; // THIS IS REDUNDANT IGNORE THIS CLASS FOR NOW
using Spill_The_Beanz_Coffee_Shop_API.Models;

namespace Spill_The_Beanz_Coffee_Shop_API.Services
{
    public class AuthService
    {
        private readonly CoffeeDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(CoffeeDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> RegisterCustomerAsync(RegisterCustomerDto dto)
        {
            if (await _context.Customers.AnyAsync(c => c.CustomerEmail == dto.CustomerEmail))
                return false;

            var customer = new Customers
            {
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerEmail == dto.Email);
            if (customer == null || !BCrypt.Net.BCrypt.Verify(dto.Password, customer.PasswordHash))
                return null;

            return GenerateJwtToken(customer);
        }

        private string GenerateJwtToken(Customers customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, customer.CustomerID.ToString()),
                new Claim(ClaimTypes.Email, customer.CustomerEmail)
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_config["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
