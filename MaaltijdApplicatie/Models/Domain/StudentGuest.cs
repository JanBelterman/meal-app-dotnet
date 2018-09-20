namespace MaaltijdApplicatie.Models.Domain {

    public class StudentGuest {

        public virtual AppUser AppUser { get; set; }
        public string AppUserId { get; set; }

        public Meal Meal { get; set; }
        public int MealId { get; set; }

    }
}
