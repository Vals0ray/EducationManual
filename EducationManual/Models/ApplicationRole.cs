using Microsoft.AspNet.Identity.EntityFramework;

namespace EducationManual.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }
    }

    public class EditRoleModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateRoleModel
    {
        public string Name { get; set; }
    }
}