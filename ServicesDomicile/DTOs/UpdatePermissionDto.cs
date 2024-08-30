using Microsoft.AspNetCore.Authorization;
using ServicesDomicile.Controllers;
using ServicesDomicile.DTOs.OtherObjects;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace ServicesDomicile.DTOs
{
    public class UpdatePermissionDto
    {
        [Required]
        public string UserName { get; set; }

        public List<Guid> CategoryIds { get; set; } = new List<Guid>();

        [Required]
        public IFormFile IdImage { get; set; }
    }


}
