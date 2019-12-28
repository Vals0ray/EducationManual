using System.ComponentModel.DataAnnotations;

namespace EducationManual.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        public string ProfilePicture { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Role { get; set; }

        [Required]
        public int SchoolId { get; set; }

        [Required]
        public string SchoolName { get; set; }

        public bool isBlocked { get; set; }

        public string Password { get; set; }

        public string returnURL { get; set; }
    }
}