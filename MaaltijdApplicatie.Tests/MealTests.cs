using MaaltijdApplicatie.Models.Domain;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MaaltijdApplicatie.Tests {

    public class MealTests {

        [Fact]
        public void Students_Can_Join_Meals_With_Space_Left() {

            // Arrange
            Meal meal = new Meal() { MaxGuests = 3, Cook = new Student() { Id = 1, Name = "Test student" } };
            Student student = new Student() { Id = 2, Name = "Test student" };

            // Act
            meal.Join(student);

            // Assert
            Guest[] guests = meal.Guests.ToArray();
            Assert.Equal(student.Id, guests[0].Student.Id);

        }

        [Fact]
        public void Students_Cannot_Join_Meals_Without_Space_Left() {

            // Arrange
            Meal meal = new Meal() { MaxGuests = 1, Cook = new Student() { Id = 1, Name = "Test student" } };
            Student student = new Student() { Id = 2, Name = "Test student" };
            Student anotherStudent = new Student() { Id = 3, Name = "Test student" };
            meal.Join(student);

            // Act
            meal.Join(anotherStudent);

            // Assert
            Guest[] guests = meal.Guests.ToArray();
            Assert.Equal(1, meal.Guests.Count); // Check that only one student is guest
            Assert.Equal(student.Id, guests[0].Student.Id); // Check that student is not overriden

        }

        [Fact]
        public void Student_Cannot_Join_Meals_Which_They_Have_Already_Joined() {

            // Arrange
            Meal meal = new Meal() { MaxGuests = 3, Cook = new Student() { Id = 1, Name = "Test student" } };
            Student student = new Student() { Id = 2, Name = "Test student" };
            meal.Join(student);

            // Act
            meal.Join(student);

            // Assert
            Assert.Equal(1, meal.Guests.Count);

        }

        [Fact]
        public void Students_Cannot_Join_Meals_Which_They_Cook() {

            // Arrange
            Student student = new Student() { Id = 1, Name = "Test student" };
            Meal meal = new Meal() { MaxGuests = 3, Cook = student};

            // Act
            meal.Join(student);

            // Assert
            Assert.Equal(0, meal.Guests.Count);

        }

        [Fact]
        public void Students_Can_Leave_Meal() {

            // Assert
            Meal meal = new Meal() { MaxGuests = 3, Cook = new Student() { Id = 1, Name = "Test student" } };
            Student student = new Student() { Id = 2, Name = "Test student" };
            Student anotherStudent = new Student() { Id = 3, Name = "Test student" };
            meal.Join(student);
            meal.Join(anotherStudent);

            // Act
            meal.Leave(student);

            // Assert
            Assert.Equal(1, meal.Guests.Count);

        }

        [Fact]
        public void Students_Cannot_Leave_Meal_Which_They_Did_Not_Join() {

            // Arrange
            Meal meal = new Meal() { MaxGuests = 3, Cook = new Student() { Id = 1, Name = "Test student" } };
            Student student = new Student() { Id = 2, Name = "Test student" };
            Student anotherStudent = new Student() { Id = 3, Name = "Test student" };
            meal.Join(student);

            // Act
            DomainMethodResult result = meal.Leave(anotherStudent);

            // Assert
            Assert.False(result.WasSuccessful);

        }

    }

}
