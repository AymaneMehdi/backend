using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.Entities
{
    public class Gig
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string CoverImage { get; set; }

        public List<string> Images { get; set; } = new List<string>();

        [Required]
        public string Desc { get; set; }

        [Required]
        public string ShortTitle { get; set; }

        [Required]
        public string ShortDesc { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; }  // Add this line

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
