using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MaaltijdApplicatie.Models.Domain {

    public class Meal {

        public int Id { get; set; }

        public DateTime DateTime { get; set; }

        [DisplayName("Naam")]
        [Required(ErrorMessage = "Vul een naam in")]
        public string Name { get; set; }

        [DisplayName("Beschrijving")]
        [Required(ErrorMessage = "Vul een beschrijving in")]
        public string Description { get; set; }

        [DisplayName("Prijs")]
        [Required(ErrorMessage = "Vul een prijs in")]
        public decimal? Price { get; set; }

        [DisplayName("Max. aantal mee eters")]
        [Required(ErrorMessage = "Vul het maximaal aantal mee eters in")]
        public int? MaxGuests { get; set; }

        public Student Cook { get; set; }

        public ICollection<Guest> Guests { get; set; } = new List<Guest>();

        public DomainMethodResult AllowedToEdit(Student student) {

            DomainMethodResult result = new DomainMethodResult();

            // If user is not the cook of this meal -> he/she is not allowed to edit
            if (Cook.Id != student.Id) {
                result.Message = "Je kan alleen maaltijden wijzigen waar je zelf kok van bent";
            } 
            // If guests have registered to this mea -> the meal cannot be edited
            else if (Guests.Count > 0) {
                result.Message = "Je kan alleen maaltijden bijwerken waar nog niemand mee eet";
            }
            // Student is allowed to edit meal
            else {
                result.Succeeded();
            }

            return result;

        }

        public DomainMethodResult Join(Student student) {

            DomainMethodResult result = new DomainMethodResult();

            // If student is cook he/she cannot register
            if (Cook.Id == student.Id) {
                result.Message = "Je kan niet mee eten aan een maaltijd die je zelf kookt";
            }
            // If student has already registered he/she cannot register a second time
            else if (Guests.Any(g => g.StudentId == student.Id)) {
                result.Message = "Je eet al mee aan deze maaltijd";
            }
            // If meal has no free space left student cannot register
            else if (Guests.Count >= MaxGuests) {
                result.Message = "Je kan niet meer mee eten, alle plekken zijn bezet";
            }
            // Student is allowed to join meal
            else {
                Guests.Add(new Guest() { Meal = this, MealId = Id, Student = student, StudentId = student.Id });
                result.Succeeded();
                result.Message = "Succesvol aangemeld";
            }

            return result;

        }

        public DomainMethodResult Leave(Student student) {

            DomainMethodResult result = new DomainMethodResult();

            // If student has not joined this meal, he/she cannot leave
            if (!Guests.Any(m => m.StudentId == student.Id)) {
                result.Message = "Je kan je niet afmelden voor een maaltijd waar je niet voor aangemeld bent";
            }
            // Student is allowed to leave meal
            else {
                Guest guest = Guests.FirstOrDefault(g => g.StudentId == student.Id);
                Guests.Remove(guest);
                result.Succeeded();
                result.Message = "Succesvol afgemeld";
            }

            return result;

        }

    }

}
