using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Müşteri adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
        [Display(Name = "Adı")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Müşteri soyadı zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
        [Display(Name = "Soyadı")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "TC Kimlik / Pasaport No zorunludur.")]
        [StringLength(20, ErrorMessage = "Kimlik no en fazla 20 karakter olabilir.")]
        [Display(Name = "TC / Pasaport No")]
        public string NationalId { get; set; } = string.Empty;

        [Display(Name = "Adres")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Müşteri Ad Soyad")]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation Properties
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}
