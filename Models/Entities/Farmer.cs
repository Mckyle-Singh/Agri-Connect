using Microsoft.AspNetCore.Identity;

namespace Agri_Connect.Models.Entities
{
    public class Farmer
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string FarmName { get; set; }
        public string Location { get; set; }

        public string UserId { get; set; }  // Foreign key to IdentityUser

        // Navigation property, if you want to access the IdentityUser object in your code
        public virtual IdentityUser User { get; set; }
    }

}
