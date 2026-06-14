using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Firma adı zorunludur.")]
        [StringLength(200)]
        [Display(Name = "Firma Adı")]
        public string CompanyName { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Yetkili Kişi")]
        public string? ContactPerson { get; set; }

        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi giriniz.")]
        [Display(Name = "E-posta")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "Geçerli bir telefon numarası giriniz.")]
        [Display(Name = "Telefon")]
        public string? Phone { get; set; }

        [Display(Name = "Adres")]
        public string? Address { get; set; }

        // Navigation property for purchase orders
        public ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
    }
}
