using System.ComponentModel.DataAnnotations;

namespace EducationManual.Models
{
    public class TaskForStudent
    {
        [Key]
        public int? TaskId { get; set; }

        public string Name { get; set; }
    }
}