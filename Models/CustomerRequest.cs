using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public class CustomerRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Müşteri")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        [Required]
        [Display(Name = "Rezervasyon")]
        public int ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public Reservation? Reservation { get; set; }

        [Required(ErrorMessage = "Talep metni zorunludur.")]
        [StringLength(500)]
        [Display(Name = "Talep Metni")]
        public string RequestText { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tarih")]
        public DateTime RequestDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Çözüldü mü?")]
        public bool IsResolved { get; set; } = false;
    }
}
