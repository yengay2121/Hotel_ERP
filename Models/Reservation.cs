using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public enum ReservationStatus
    {
        [Display(Name = "Beklemede")]
        Pending,
        [Display(Name = "Onaylandı")]
        Confirmed,
        [Display(Name = "İptal Edildi")]
        Cancelled,
        [Display(Name = "Tamamlandı")]
        Completed
    }

    public class Reservation
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Oda seçimi zorunludur.")]
        [Display(Name = "Oda")]
        public int RoomId { get; set; }

        [Required(ErrorMessage = "Müşteri seçimi zorunludur.")]
        [Display(Name = "Müşteri")]
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "Giriş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Giriş Tarihi")]
        public DateTime CheckInDate { get; set; }

        [Required(ErrorMessage = "Çıkış tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "Çıkış Tarihi")]
        public DateTime CheckOutDate { get; set; }

        [Display(Name = "Toplam Tutar")]
        public decimal TotalPrice { get; set; }

        [Display(Name = "Rezervasyon Durumu")]
        public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

        // Navigation Properties
        public Room? Room { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
