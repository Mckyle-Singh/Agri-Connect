using System.ComponentModel.DataAnnotations;

namespace Agri_Connect.Models.ViewModels
{
    public class EditFarmerViewModel
    {
        
            public int Id { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

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
