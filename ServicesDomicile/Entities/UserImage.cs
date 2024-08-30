using ServicesDomicile.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServicesDomicile.Entities
{
    public class UserImage
    {
        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
