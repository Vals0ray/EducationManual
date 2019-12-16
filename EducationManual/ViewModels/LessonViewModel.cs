using EducationManual.Models;
using System.ComponentModel.DataAnnotations;

namespace EducationManual.ViewModels
{
    public class LessonViewModel
    {
        [Key]
        public int LessonId { get; set; }

        public string Name { get; set; }

        public int? TestId { get; set; }

        public Student Student { get; set; }
    }
}