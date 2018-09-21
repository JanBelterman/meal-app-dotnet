using MaaltijdApplicatie.Models.Context;
using MaaltijdApplicatie.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MaaltijdApplicatie.Models.Repositories {

    public class DbStudentRepository : IStudentRepository {

        private AppDomainDbContext database;

        public DbStudentRepository(AppDomainDbContext database) {
            this.database = database;
        }

        // Get student from user id
        public Student GetStudent(string userId) =>
            database.Students.Include(s => s.CookOfMeals).Include(s => s.GuestOfMeals).FirstOrDefault(s => s.UserId == userId);

        // Stores a student
        public void SaveStudent(Student student) {

            // If id = 0 -> student does not exist -> create it
            if (student.Id == 0) {
                // Create student
                database.Students.Add(student);

            } else { // If id = not 0 -> student exists -> update it
                // Get student
                Student dbEntry = database.Students.FirstOrDefault(m => m.Id == student.Id);
                // Update properties
                if (dbEntry != null) {
                    dbEntry.Name = student.Name;
                    dbEntry.UserId = student.UserId;
                }

            }

            // Save changes
            database.SaveChanges();

        }

    }

}
