using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.DTOs
{
    public class UpdatePermissionOwnerDto
    {
        [Required]
        public string UserName { get; set; }
    }
}
