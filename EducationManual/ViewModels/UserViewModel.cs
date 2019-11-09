using EducationManual.Models;
using System.ComponentModel.DataAnnotations;

namespace EducationManual.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string Role { get; set; }

        public int? SchoolId { get; set; }

        public bool isBlocked { get; set; }
    }
}