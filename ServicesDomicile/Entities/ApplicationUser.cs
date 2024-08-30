using Microsoft.AspNetCore.Identity;

namespace ServicesDomicile.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public virtual ICollection<UserImage> UserImages { get; set; }
        public virtual ICollection<UserCategory> UserCategories { get; set; }
    }
}
