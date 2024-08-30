using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.DTOs
{
    public class UpdateGigDto
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ShortTitle { get; set; }
        public string ShortDesc { get; set; }
        public Guid CategoryId { get; set; } // Ensure this is correct if needed
    }

}
