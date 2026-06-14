using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Departman adı zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Departman Adı")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Açıklama")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Otel seçimi zorunludur.")]
        [Display(Name = "Otel")]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel? Hotel { get; set; }

        // Navigation properties
        // E.g. List of employees in this department, but keeping it simple for now as Employee already has a HotelId.
    }
}
