using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.ViewModels {

    public class LoginViewModel {

        [DisplayName("Gebruikersnaam")]
        [Required(ErrorMessage = "Vul een gebruikersnaam in")]
        public string Username { get; set; }

        [DisplayName("Wachtwoord")]
        [UIHint("password")]
        [Required(ErrorMessage = "Vul een wachtwoord in")]
        public string Password { get; set; }

    }

}
