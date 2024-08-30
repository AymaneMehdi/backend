using ServicesDomicile.Entities;
using System;

namespace ServicesDomicile.Entities
{
    public class UserCategory
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid CategoryId { get; set; }

        public ApplicationUser User { get; set; }
        public Category Category { get; set; }
    }
}
