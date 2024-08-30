using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.Entities
{
    public class Category
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<UserCategory> UserCategories { get; set; }
    }
}
