using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.ViewModels {

    public class AppUserRegisterViewModel {

        [DisplayName("Gebruikersnaam")]
        [Required(ErrorMessage = "Vul een gebruikersnaam in")]
        public string Username { get; set; }

        [DisplayName("Wachtwoord")]
        [UIHint("password")]
        [Required(ErrorMessage = "Vul een wachtwoord in")]
        public string Password { get; set; }

        [DisplayName("Naam")]
        [Required(ErrorMessage = "Vul een naam in")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Vul een email address in")]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Ongeldig email address")]
        public string Email { get; set; }

        [DisplayName("Telefoon nummer")]
        [Required(ErrorMessage = "Vul een telefoon nummer in")]
        [RegularExpression(@"^(?=^.{10,11}$)0\d*-?\d*$", ErrorMessage ="Ongeldig telefoon nummer")]
        public string Number { get; set; }


    }

}
