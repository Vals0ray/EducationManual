using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EducationManual.Models
{
    public class School
    {
        [Key]
        public int SchoolId { get; set; }

        public string Name { get; set; }

        public string SchoolAdminId { get; set; }

        public ICollection<Classroom> Classrooms { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public School()
        {
            Classrooms = new List<Classroom>();
            ApplicationUsers = new List<ApplicationUser>();
        }
    }
}