using System.Collections.Generic;

namespace EducationManual.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }
        public Classroom()
        {
            Students = new List<Student>();
        }
    }
}