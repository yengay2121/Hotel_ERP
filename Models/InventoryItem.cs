using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public class InventoryItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ürün adı zorunludur.")]
        [StringLength(150)]
        [Display(Name = "Ürün Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "SKU kodu zorunludur.")]
        [StringLength(50)]
        [Display(Name = "SKU (Stok Kodu)")]
        public string SKU { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stok miktarı zorunludur.")]
        [Display(Name = "Stok Miktarı")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Birim fiyatı zorunludur.")]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Birim Fiyatı")]
        public decimal UnitPrice { get; set; }

        [Required(ErrorMessage = "Otel seçimi zorunludur.")]
        [Display(Name = "Otel")]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel? Hotel { get; set; }
    }
}
