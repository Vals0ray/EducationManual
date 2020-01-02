using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web;

namespace EducationManual.Models
{
    public class ContextInitializer : CreateDatabaseIfNotExists<ApplicationContext>
    {
        protected override void Seed(ApplicationContext db)
        {
            ApplicationUserManager manager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));
            var RoleManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            RoleManager.Create(new ApplicationRole() { Name = "SuperAdmin" });
            RoleManager.Create(new ApplicationRole() { Name = "SchoolAdmin" });
            RoleManager.Create(new ApplicationRole() { Name = "Teacher" });
            RoleManager.Create(new ApplicationRole() { Name = "Student" });

            ApplicationUser user = new ApplicationUser()
            {
                FirstName = "Yaroslav",
                SecondName = "Dukhnych",
                Email = "SuperAdmin",
                UserName = "SuperAdmin",
                SchoolId = 1
            };

            db.Schools.Add(new School() { Name = "AdminSchool", ApplicationUsers = new List<ApplicationUser>() { user } });

            manager.Create(user, "111111");

            manager.AddToRole(user.Id, "SuperAdmin");

            db.SaveChanges();
        }
    }
}