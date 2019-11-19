using System.ComponentModel.DataAnnotations;

namespace EducationManual.ViewModels
{
    public class StudentViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string SecondName { get; set; }

        public int? TestId { get; set; }
    }
}