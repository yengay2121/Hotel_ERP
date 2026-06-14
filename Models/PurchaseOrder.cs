using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public enum PurchaseOrderStatus
    {
        [Display(Name = "Beklemede")]
        Pending = 0,
        [Display(Name = "Onaylandı")]
        Approved = 1,
        [Display(Name = "Teslim Alındı")]
        Received = 2,
        [Display(Name = "İptal Edildi")]
        Cancelled = 3
    }

    public class PurchaseOrder
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tedarikçi seçimi zorunludur.")]
        [Display(Name = "Tedarikçi")]
        public int SupplierId { get; set; }

        [ForeignKey("SupplierId")]
        public Supplier? Supplier { get; set; }

        [Required(ErrorMessage = "Otel seçimi zorunludur.")]
        [Display(Name = "Otel")]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel? Hotel { get; set; }

        [Required]
        [Display(Name = "Sipariş Tarihi")]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ürün seçimi zorunludur.")]
        [Display(Name = "Envanter Ürünü")]
        public int InventoryItemId { get; set; }

        [ForeignKey("InventoryItemId")]
        public InventoryItem? InventoryItem { get; set; }

        [Required]
        [Range(1, 100000)]
        [Display(Name = "Miktar")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Toplam Tutar")]
        public decimal TotalAmount { get; set; }

        [Required]
        [Display(Name = "Sipariş Durumu")]
        public PurchaseOrderStatus Status { get; set; } = PurchaseOrderStatus.Pending;
    }
}
