using BeFit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Mix> Mix { get; set; } = default!;
        public DbSet<Typy> Typy { get; set; } = default!;
        public DbSet<Sesje> Sesje { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Id = "ADULT_ROLE",
                   Name = "Adult",
                   NormalizedName = "ADULT",
                   ConcurrencyStamp = "ADULT"
               },
               new IdentityRole
               {
               Id = "ADMIN_ROLE",
               Name = "Admin",
               NormalizedName = "ADMIN",
               ConcurrencyStamp = "ADMIN"
               }
            );
        }
    }
}
