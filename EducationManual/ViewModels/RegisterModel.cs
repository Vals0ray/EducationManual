using System.ComponentModel.DataAnnotations;

namespace EducationManual.ViewModels
{
    public class RegisterModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords are different")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}