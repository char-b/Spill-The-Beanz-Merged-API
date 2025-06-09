public class OrderStatusPatchDTO //This DTO is used to change the order status on admin side
{
    public int OrderId { get; set; }
    public string? OrderType { get; set; }  // ? Makes nullable the value nullable
    public string? OrderStatus { get; set; }
}
