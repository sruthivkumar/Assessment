using System.ComponentModel.DataAnnotations;

namespace ProductOrderAPI.DTOs.Prouct
{
    public class ProductCreateDto
    {
        [Required]
        public string Name { get; set; } = null!;
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }
    }
}
