using MaaltijdApplicatie.Models.Domain;
using System.Linq;
using Xunit;

namespace MaaltijdApplicatie.Tests {

    public class MealTests {

        [Fact]
        public void Students_Can_Join_Meal_With_Space_Left() {

            // Arrange
            Meal meal = new Meal() { MaxGuests = 3, Cook = new Student() { Id = 2, Name = "Test student" } };
            Student student = new Student() { Id = 1, Name = "Test student" };

            // Act
            meal.Join(student);

            // Assert
            Guest[] guests = meal.Guests.ToArray();
            Assert.Equal(guests[0].Student.Id, student.Id);

        }

    }

}
