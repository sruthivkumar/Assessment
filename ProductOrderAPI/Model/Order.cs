using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ProductOrderAPI.Model
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
        [Range(1, int.MaxValue)]
        public int Qty { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

[Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
    }
}
