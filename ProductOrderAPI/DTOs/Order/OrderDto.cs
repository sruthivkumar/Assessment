namespace ProductOrderAPI.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Qty { get; set; }
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}