using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace ServicesDomicile.DTOs
{
    public class CreateGigDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public IFormFile CoverImage { get; set; }

        [Required]
        public List<IFormFile> Images { get; set; }

        [Required]
        public string Desc { get; set; }

        [Required]
        public string ShortTitle { get; set; }

        [Required]
        public string ShortDesc { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }

}
