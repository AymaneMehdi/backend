using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.DTOs.Owner
{
    public class UpdateCategoryDto
    {
        [Required]
        public string name { get; set; }
    }
}
