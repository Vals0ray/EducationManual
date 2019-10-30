namespace EducationManual.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Age { get; set; }

        public string Phone { get; set; }

        public int? ClassroomId { get; set; }
        public Classroom Classroom { get; set; }
    }
}