using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public class CustomerFeedback
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

        [Required(ErrorMessage = "Puanlama zorunludur.")]
        [Range(1, 5, ErrorMessage = "Puan 1 ile 5 arasında olmalıdır.")]
        [Display(Name = "Puan (1-5)")]
        public int Rating { get; set; }

        [StringLength(1000)]
        [Display(Name = "Yorum")]
        public string Comment { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Tarih")]
        public DateTime FeedbackDate { get; set; } = DateTime.Now;
    }
}
