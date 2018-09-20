using MaaltijdApplicatie.Models.Domain;

namespace MaaltijdApplicatie.Models.Repositories {

    public interface IStudentRepository {

        Student GetStudent(string userId);

        void SaveStudent(Student student);

    }

}
