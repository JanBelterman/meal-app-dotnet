using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Users.Models.Context {

    public class AppIdentityDbContext : IdentityDbContext<AppUser> {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options) { }
        
        public DbSet<Meal> Meals { get; set; }
        public DbSet<StudentGuest> StudentGuests { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            // StudentGuest primary key (also sets many : many realtion between student & meal)
            builder.Entity<StudentGuest>().HasKey(t => new { t.MealId, t.AppUserId });

        }

        protected UserManager<AppUser> UserManager { get; set; }

    }

}
