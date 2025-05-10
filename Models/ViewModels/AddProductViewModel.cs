using Agri_Connect.Enums;
using System.ComponentModel.DataAnnotations;

namespace Agri_Connect.Models.ViewModels
{
    public class AddProductViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "Product Type")]
        public ProductType Type { get; set; }

        [Display(Name = "Image URL")]
        public string? ProductImageUrl { get; set; } // Stored image URL in DB

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string Description { get; set; }

        public int FarmerId { get; set; }  // This can b
    }
}
