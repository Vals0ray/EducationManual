using Microsoft.AspNet.Identity.EntityFramework;

namespace EducationManual.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? SchoolId { get; set; }

        public School School { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public byte[] ProfilePicture { get; set; }

        public Student Student { get; set; }

        public ApplicationUser()
        {
        }
    }
}