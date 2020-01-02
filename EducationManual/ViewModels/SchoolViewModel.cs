using EducationManual.Models;
using System.ComponentModel.DataAnnotations;

namespace EducationManual.ViewModels
{
    public class SchoolViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ApplicationUser SchoolAdmin { get; set; }
    }
}