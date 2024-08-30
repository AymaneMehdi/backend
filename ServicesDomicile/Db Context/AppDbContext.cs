using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ServicesDomicile.Entities;

namespace ServicesDomicile.Db_Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Gig> Gigs { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<UserImage> UserImages { get; set; }

        public DbSet<UserCategory> UserCategories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Gig entity
            modelBuilder.Entity<Gig>()
                .Property(g => g.Images)
                .HasConversion(
                    v => string.Join(";", v),
                    v => v.Split(";", System.StringSplitOptions.RemoveEmptyEntries).ToList())
                .Metadata.SetValueComparer(new ValueComparer<List<string>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            //modelBuilder.Entity<Gig>()
            //    .Property(g => g.Features)
            //    .HasConversion(
            //        v => string.Join(";", v),
            //        v => v.Split(";", System.StringSplitOptions.RemoveEmptyEntries).ToList())
            //    .Metadata.SetValueComparer(new ValueComparer<List<string>>(
            //        (c1, c2) => c1.SequenceEqual(c2),
            //        c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            //        c => c.ToList()));

            // Specify precision and scale for the Price property
            //modelBuilder.Entity<Gig>()
            //    .Property(g => g.Price)
            //    .HasColumnType("decimal(18,2)");

            // Configure the relationship between Gig and ApplicationUser
            modelBuilder.Entity<Gig>()
                .HasOne(g => g.User)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Gig>()
               .HasOne(g => g.Category)
               .WithMany()
               .HasForeignKey(g => g.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserCategory>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserCategories)
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserCategory>()
                .HasOne(uc => uc.Category)
                .WithMany(c => c.UserCategories)
                .HasForeignKey(uc => uc.CategoryId);

            modelBuilder.Entity<UserImage>()
                .HasOne(ui => ui.User)
                .WithMany(u => u.UserImages)
                .HasForeignKey(ui => ui.UserId);




        }
    }
}
