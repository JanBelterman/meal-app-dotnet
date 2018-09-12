using System.ComponentModel.DataAnnotations;

namespace MaaltijdApplicatie.Models.ViewModels {

    public class AppUserLoginViewModel {

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }

}
