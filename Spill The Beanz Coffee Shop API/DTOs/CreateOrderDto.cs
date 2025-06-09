﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spill_The_Beanz_Coffee_Shop_API.DTOs 
    {
        public class CreateOrderDto
        {
        // [Required]
        // public int CustomerId { get; set; } //we are getting it from the token :)
        public string OrderType { get; set; } = null!;
       
        public decimal TotalAmount { get; set; }
        public decimal TaxAmount { get; set; }
       // public decimal DiscountAmount { get; set; }
        
        public decimal FinalAmount { get; set; }
        public string OrderStatus { get; set; }
       // public string? SpecialInstructions { get; set; }

        [Required]
            public List<OrderItemDto> Items { get; set; } = new();
        }

    }


