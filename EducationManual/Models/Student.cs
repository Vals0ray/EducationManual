using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EducationManual.Models
{
    public class Student
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        public int ClassroomId { get; set; }
        public Classroom Classroom { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}