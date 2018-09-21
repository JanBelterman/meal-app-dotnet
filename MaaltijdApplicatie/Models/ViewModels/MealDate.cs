using MaaltijdApplicatie.Models.Domain;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.ViewModels {

    public class MealDate {

        // Date information
        public DateTime Date { get; set; }
        public string MonthString { get; set; }
        public string DayOfWeekString { get; set; }

        // Time (can be altered to user export and import view models)
        [DisplayName("Tijd")]
        [Required]
        [DataType(DataType.Time)]
        public DateTime Time { get; set; }

        // Meal domain model
        public Meal Meal { get; set; }

        // Meta information for meal -> used to decide which buttons to display
        public bool UserHasJoined { get; set; } = false;
        public bool UserIsCook { get; set; } = false;
        public bool MealIsFull { get; set; } = false;

    }

}
