using Microsoft.AspNet.Identity.EntityFramework;

namespace EducationManual.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Year { get; set; }

        public int Age { get; set; }

        public ApplicationUser()
        {
        }
    }
}