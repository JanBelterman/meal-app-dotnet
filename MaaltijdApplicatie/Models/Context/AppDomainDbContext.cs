using MaaltijdApplicatie.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace MaaltijdApplicatie.Models.Context {

    public class AppDomainDbContext : DbContext {

        public AppDomainDbContext(DbContextOptions<AppDomainDbContext> options) : base(options) { }

        public DbSet<Meal> Meals { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Guest> Guests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            // Guest primary key (also sets many : many relation between student & meal)
            builder.Entity<Guest>().HasKey(t => new { t.MealId, t.StudentId });

        }

    }

}
