using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursova.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Display(Name = "Изображение")]
        public string ImageUrl { get; set; } = string.Empty;
    }
}
