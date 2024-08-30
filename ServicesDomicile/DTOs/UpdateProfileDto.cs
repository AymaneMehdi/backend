using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.DTOs
{
    public class UpdateProfileDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string City { get; set; }
    }
}
