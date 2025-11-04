using System.ComponentModel.DataAnnotations;

namespace ProductOrderAPI.DTOs.Order
{
    public class OrderCreateDto
    {
        [Required]
        public int ProductId { get; set; }
        [Range(1, int.MaxValue)]
        public int Qty { get; set; }
    }
}
