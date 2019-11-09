using System.ComponentModel.DataAnnotations;

namespace EducationManual.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public int? ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }
}