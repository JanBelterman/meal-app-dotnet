using MaaltijdApplicatie.Controllers;
using MaaltijdApplicatie.Models.Domain;
using MaaltijdApplicatie.Models.Repositories;
using MaaltijdApplicatie.Models.ViewModels;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MaaltijdApplicatie.Tests {

    public class MealControllerTests {

        /*[Fact]
        public void Can_Join_Meal_That_Has_Space() {

            // Arrange
            Meal meal = new Meal { MaxGuests = 4 };
            Mock<IMealRepository> mealMock = new Mock<IMealRepository>();
            mealMock.Setup(m => m.GetMeal(1)).Returns(meal);
            Student student = new Student { Id = 1 };
            Mock<IStudentRepository> studentMock = new Mock<IStudentRepository>();
            studentMock.Setup(m => m.GetStudent("1")).Returns(student);
            MealController target = new MealController(mealMock.Object, studentMock.Object);

            // Act
            target.Join(1);

            // Assert
            mealMock.Verify(m => m.JoinMeal(meal, student), Times.Once);

        }*/

        /*[Fact]
        public void Lists_Only_Meals_For_Two_Weeks() {

            // Arrange
            Mock<IMealRepository> mealMock = new Mock<IMealRepository>();
            Mock<IStudentRepository> studentMock = new Mock<IStudentRepository>();
            MealController target = new MealController(mealMock.Object, studentMock.Object);

            // Act
            MealDate[] result = GetViewModel<IEnumerable<MealDate>>(target.List())?.ToArray();

            // Assert

        }*/

    }

}
