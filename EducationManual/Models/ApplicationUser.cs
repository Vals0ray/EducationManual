using Microsoft.AspNet.Identity.EntityFramework;

namespace EducationManual.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? SchoolId { get; set; }
        public School School { get; set; }

        public ApplicationUser()
        {
        }
    }
}