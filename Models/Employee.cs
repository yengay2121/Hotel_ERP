using System.ComponentModel.DataAnnotations;

namespace HotelERP.Models
{
    public enum EmployeePosition
    {
        [Display(Name = "Müdür")]
        Manager,
        [Display(Name = "Resepsiyonist")]
        Receptionist,
        [Display(Name = "Kat Hizmetleri")]
        Housekeeping,
        [Display(Name = "Aşçı")]
        Chef,
        [Display(Name = "Güvenlik")]
        Security
    }

    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Personel adı zorunludur.")]
        [StringLength(50, ErrorMessage = "Ad en fazla 50 karakter olabilir.")]
        [Display(Name = "Adı")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Personel soyadı zorunludur.")]
        [StringLength(50, ErrorMessage = "Soyad en fazla 50 karakter olabilir.")]
        [Display(Name = "Soyadı")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pozisyon zorunludur.")]
        [Display(Name = "Pozisyon")]
        public EmployeePosition Position { get; set; }

        [Required(ErrorMessage = "Maaş zorunludur.")]
        [Range(0, 1000000, ErrorMessage = "Maaş 0'dan büyük olmalıdır.")]
        [Display(Name = "Maaş")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "İşe giriş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        [Display(Name = "İşe Giriş Tarihi")]
        public DateTime HireDate { get; set; }

        [Required(ErrorMessage = "Otel seçimi zorunludur.")]
        [Display(Name = "Çalıştığı Otel")]
        public int HotelId { get; set; }

        [Required(ErrorMessage = "Departman seçimi zorunludur.")]
        [Display(Name = "Departman")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefon zorunludur.")]
        [Phone(ErrorMessage = "Geçersiz telefon numarası.")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; } = string.Empty;

        [Display(Name = "Personel Ad Soyad")]
        public string FullName => $"{FirstName} {LastName}";

        // Navigation Properties
        public Hotel? Hotel { get; set; }
        public Department? Department { get; set; }
    }
}
