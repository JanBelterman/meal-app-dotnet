using MaaltijdApplicatie.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Users.Models.Context {

    public class AppIdentityDbContext : IdentityDbContext<AppUser> {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options)
        : base(options) { }
        
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealStudent> MealStudents { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {

            base.OnModelCreating(builder);

            builder.Entity<MealStudent>().HasKey(t => new { t.MealId, t.AppUserId });

            builder.Entity<AppUser>().HasMany<MealStudent>(e => e.GuestOfMeals).WithOne(e => e.AppUser);
            builder.Entity<Meal>().HasOne<AppUser>(e => e.StudentCook).WithMany(e => e.CookOfMeals);
            builder.Entity<Meal>().HasMany<MealStudent>(e => e.StudentsGuests).WithOne(e => e.Meal);
            builder.Entity<Meal>().HasIndex(e => e.DateTime).IsUnique();


        }

        protected UserManager<AppUser> UserManager { get; set; }

    }

}
