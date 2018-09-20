namespace MaaltijdApplicatie.Models.Domain {

    public class Guest {

        public Student Student { get; set; }
        public int StudentId { get; set; }

        public Meal Meal { get; set; }
        public int MealId { get; set; }

    }
}
