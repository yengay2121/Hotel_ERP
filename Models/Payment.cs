using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public enum PaymentMethod
    {
        [Display(Name = "Kredi Kartı")]
        CreditCard,
        [Display(Name = "Nakit")]
        Cash,
        [Display(Name = "Banka Havalesi")]
        BankTransfer
    }

    public enum PaymentStatus
    {
        [Display(Name = "Beklemede")]
        Pending,
        [Display(Name = "Ödendi")]
        Paid,
        [Display(Name = "İade Edildi")]
        Refunded
    }

    public class Payment
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rezervasyon seçimi zorunludur.")]
        [Display(Name = "Rezervasyon")]
        public int ReservationId { get; set; }

        [Required(ErrorMessage = "Tutar zorunludur.")]
        [Range(0.01, 1000000, ErrorMessage = "Tutar 0'dan büyük olmalıdır.")]
        [Display(Name = "Ödeme Tutarı")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Ödeme tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Ödeme Tarihi")]
        public DateTime PaymentDate { get; set; }

        [Required(ErrorMessage = "Ödeme yöntemi zorunludur.")]
        [Display(Name = "Ödeme Yöntemi")]
        public PaymentMethod PaymentMethod { get; set; }

        [Required(ErrorMessage = "Ödeme durumu zorunludur.")]
        [Display(Name = "Ödeme Durumu")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

        // Navigation Properties
        public Reservation? Reservation { get; set; }
    }
}
