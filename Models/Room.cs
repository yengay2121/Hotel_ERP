using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public enum RoomType
    {
        [Display(Name = "Tek Kişilik (Single)")]
        Single,
        [Display(Name = "Çift Kişilik (Double)")]
        Double,
        [Display(Name = "Süit (Suite)")]
        Suite,
        [Display(Name = "Deluxe")]
        Deluxe
    }

    public class Room
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Oda numarası zorunludur.")]
        [Display(Name = "Oda Numarası")]
        public string RoomNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Oda tipi zorunludur.")]
        [Display(Name = "Oda Tipi")]
        public RoomType RoomType { get; set; }

        [Required(ErrorMessage = "Gecelik ücret zorunludur.")]
        [Range(0.01, 100000, ErrorMessage = "Gecelik ücret 0'dan büyük olmalıdır.")]
        [Display(Name = "Gecelik Ücret")]
        public decimal PricePerNight { get; set; }

        [Display(Name = "Müsait mi?")]
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Otel seçimi zorunludur.")]
        [Display(Name = "Otel")]
        public int HotelId { get; set; }

        // Navigation Properties
        public Hotel? Hotel { get; set; }
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
