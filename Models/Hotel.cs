using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public class Hotel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Otel adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Otel adı en fazla 100 karakter olabilir.")]
        [Display(Name = "Otel Adı")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Adres zorunludur.")]
        [StringLength(200, ErrorMessage = "Adres en fazla 200 karakter olabilir.")]
        [Display(Name = "Adres")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Range(1, 5, ErrorMessage = "Yıldız sayısı 1 ile 5 arasında olmalıdır.")]
        [Display(Name = "Yıldız Sayısı")]
        public int Stars { get; set; }

        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        // Navigation Properties
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
