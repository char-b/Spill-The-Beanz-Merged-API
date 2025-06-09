
namespace Spill_The_Beanz_Coffee_Shop_API.DTOs
{
    public class OrderDto //This is to fetch customer order details. Used in OrdersController
    {        
        public string OrderType { get; set; }
        public DateTime OrderDate { get; set; }
        public string? SpecialInstructions { get; set; }
        public string OrderStatus { get; set; }
        public decimal FinalAmount { get; set; }
    }
}
