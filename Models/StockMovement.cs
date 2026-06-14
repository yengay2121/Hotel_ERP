using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public class StockMovement
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Envanter kalemi seçimi zorunludur.")]
        [Display(Name = "Envanter Kalemi")]
        public int InventoryItemId { get; set; }

        [ForeignKey("InventoryItemId")]
        public InventoryItem? InventoryItem { get; set; }

        [Required(ErrorMessage = "Miktar zorunludur. (Giriş için pozitif, Çıkış için negatif değer)")]
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }

        [Required]
        [Display(Name = "Tarih")]
        public DateTime MovementDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Açıklama zorunludur.")]
        [StringLength(200, ErrorMessage = "Açıklama en fazla 200 karakter olabilir.")]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;
    }
}
