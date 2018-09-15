using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.ViewModels {

    public class AppUserLoginViewModel {

        [Required]
        public string Username { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }

    }

}
