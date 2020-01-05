using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationManual.Models
{
    public class Classroom
    {
        [Key]
        public int ClassroomId { get; set; }

        [Required]
        public string Name { get; set; }

        public int SchoolId { get; set; }
        public School School { get; set; }

        public ICollection<Student> Students { get; set; }
        public Classroom()
        {
            Students = new List<Student>();
        }
    }
}