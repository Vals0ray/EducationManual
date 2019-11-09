using EducationManual.Models;

namespace EducationManual.ViewModels
{
    public class SchoolViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ApplicationUser SchoolAdmin { get; set; }
    }
}