using System.ComponentModel.DataAnnotations;

namespace Agri_Connect.Models.ViewModels
{
    public class RegisterFarmerViewModel
    {
        public int Id { get; set; } // Needed for editing existing farmers
        // Identity Fields
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

        // Farmer Profile Fields
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Farm Name")]
        public string FarmName { get; set; }

        [Required]
        public string Location { get; set; }
    }
}
