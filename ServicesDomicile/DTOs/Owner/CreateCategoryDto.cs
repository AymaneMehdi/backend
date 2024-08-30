using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.DTOs.Owner
{
    public class CreateCategoryDto
    {
        [Required]
        public string name { get; set; }
    }
}
