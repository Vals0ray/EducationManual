using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EducationManual.Models;
using EducationManual.Services;
using Microsoft.AspNet.Identity.Owin;

namespace EducationManual.Controllers
{
    [Authorize]
    public class TeacherController : Controller
    {
        private ApplicationRoleManager RoleManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        private ApplicationUserManager UserManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IUserService _userService;

        public TeacherController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult List()
        {
            // Look up the role
            string roleName = "Teacher";
            var role = RoleManager.Roles.Single(r => r.Name == roleName);

            // Find the users in that role
            var roleUsers = UserManager.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id) && u.SchoolId == DataSave.SchoolId);

            return View(roleUsers.ToList());
        }

        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Delete(ApplicationUser user)
        {
            if (user == null) return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null) return HttpNotFound();

            await _userService.DeleteUserAsync(id);

            return RedirectToAction("List");
        }
    }
}