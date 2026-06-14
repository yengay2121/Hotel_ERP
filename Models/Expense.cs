using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelERP.Models
{
    public enum ExpenseCategory
    {
        [Display(Name = "Faturalar")]
        Utilities = 0,
        [Display(Name = "Personel Maaşları")]
        Payroll = 1,
        [Display(Name = "Bakım ve Onarım")]
        Maintenance = 2,
        [Display(Name = "Pazarlama ve Reklam")]
        Marketing = 3,
        [Display(Name = "Diğer")]
        Other = 4
    }

    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Otel seçimi zorunludur.")]
        [Display(Name = "Otel")]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel? Hotel { get; set; }

        [Required(ErrorMessage = "Gider açıklaması zorunludur.")]
        [StringLength(200)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Tutar")]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Gider Tarihi")]
        public DateTime ExpenseDate { get; set; } = DateTime.Now;

        [Required]
        [Display(Name = "Kategori")]
        public ExpenseCategory Category { get; set; } = ExpenseCategory.Other;
    }
}
