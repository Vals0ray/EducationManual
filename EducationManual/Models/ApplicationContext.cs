using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace EducationManual.Models
{
    public class ApplicationContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<School> Schools { get; set; }

        public DbSet<Classroom> Classrooms { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<TaskForStudent> TaskForStudents { get; set; }

        public ApplicationContext() : base("IdentityDb") { }

        public static ApplicationContext Create()
        {
            return new ApplicationContext();
        }

        //public System.Data.Entity.DbSet<EducationManual.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}