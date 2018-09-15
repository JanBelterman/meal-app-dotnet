using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.ViewModels {

    public class AppUserRegisterViewModel {

        [Required]
        public string Username { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$", ErrorMessage = "Ongeldig email address")]
        public string Email { get; set; }
        
        [Required]
        [RegularExpression(@"^(?=^.{10,11}$)0\d*-?\d*$", ErrorMessage ="Ongeldig telefoon nummer")]
        public string Number { get; set; }


    }

}
