using Microsoft.AspNetCore.Identity;

namespace Agri_Connect.Models.Entities
{
    public class Farmer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FarmName { get; set; }
        public string Location { get; set; }

        // Foreign key to IdentityUser
        public string UserId { get; set; }  

        // Navigation property for IdentityUser
        public virtual IdentityUser User { get; set; }

        // Navigation property to Products
        public virtual ICollection<Product> Products { get; set; }
    }

}
