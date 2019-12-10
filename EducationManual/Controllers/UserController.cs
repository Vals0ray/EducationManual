using EducationManual.Models;
using EducationManual.Services;
using EducationManual.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EducationManual.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private ApplicationRoleManager RoleManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();

        private readonly IUserService _userService;
        private readonly ISchoolService _schoolService;

        public UserController(IUserService userService, ISchoolService schoolService)
        {
            _userService = userService;
            _schoolService = schoolService;
        }

        public async Task<ActionResult> Details(string userId, string returnURL)
        {
            if (string.IsNullOrEmpty(userId)) return HttpNotFound();

            var user = await UserManager.FindByIdAsync(userId);
            var userRole = await UserManager.GetRolesAsync(user.Id);
            var isBlocked = await UserManager.GetLockoutEnabledAsync(user.Id);

            UserViewModel userView = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = userRole.First(),
                SchoolId = user.SchoolId,
                isBlocked = isBlocked ? true : false
            };
            List<string> roles;

            roles = userRole.First() == "Student" ?
                new List<string>() { "Student" }  :
                new List<string>() { "SuperAdmin", "SchoolAdmin", "Teacher" };
            
            SelectList items = new SelectList(roles, userRole.First());
            ViewBag.Items = items;
            ViewBag.ReturnURL = returnURL;
            return View(userView);
        }

        [HttpPost]
        public async Task<ActionResult> Details(UserViewModel model, string returnURL)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            var userRole = (await UserManager.GetRolesAsync(model.Id)).First();

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    user.FirstName = model.FirstName;
                    user.SecondName = model.SecondName;
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;

                    // If user role has changed
                    if (model.Role != userRole)
                    {
                        await ChangeRole(user.Id, userRole, model.Role);

                        // If we set new school admin, set new school link to admin
                        if (model.Role == "SchoolAdmin")
                        {
                            var school = await _schoolService.GetSchoolAsync((int)model.SchoolId);

                            // Change old school admin to teacher
                            if (school.SchoolAdminId != null)
                            {
                                var oldSchoolAdmin = await UserManager.FindByIdAsync(school.SchoolAdminId);
                                await ChangeRole(oldSchoolAdmin.Id, "SchoolAdmin", "Teacher");
                                await UserManager.UpdateAsync(oldSchoolAdmin);
                            }

                            school.SchoolAdminId = user.Id;
                            await _schoolService.UpdateSchoolAsync(school);
                        }
                        // If we change school admin role, delete in school link to admin
                        else if (model.Role != "SchoolAdmin" && userRole == "SchoolAdmin")
                        {
                            var school = await _schoolService.GetSchoolAsync((int)model.SchoolId);
                            if (school.SchoolAdminId != null)
                            {
                                school.SchoolAdminId = null;
                                await _schoolService.UpdateSchoolAsync(school);
                            }
                        }
                    }

                    user.SchoolId = (int)model.SchoolId;
                    await Block(user.Id, model.isBlocked);

                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return Redirect(returnURL);
                    }
                    else
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
            }

            List<string> roles;
            roles = userRole == "Student" ?
                new List<string>() { "Student" } :
                new List<string>() { "SuperAdmin", "SchoolAdmin", "Teacher" };

            ViewBag.Items = new SelectList(roles);
            ViewBag.ReturnURL = returnURL;
            return View(model);
        }

        private async Task ChangeRole(string userId, string fromRole, string toRole)
        {
            await UserManager.RemoveFromRoleAsync(userId, fromRole);
            await UserManager.AddToRolesAsync(userId, toRole);
        }

        private async Task Block(string userId, bool isBlocked)
        {
            if (!isBlocked)
            {
                await UserManager.SetLockoutEnabledAsync(userId, false);
            }
            else
            {
                await UserManager.SetLockoutEnabledAsync(userId, true);
            }
        }

        public ActionResult List(string usersRole)
        {
            if(string.IsNullOrEmpty(usersRole)) return HttpNotFound();

            // Look up the role
            var role = RoleManager.Roles.Single(r => r.Name == usersRole);

            // Find the users in that role
            var roleUsers = UserManager.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .Include(u => u.School);

            ViewBag.UsersRole = usersRole;
            return View(roleUsers.ToList());
        }

        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Delete(UserViewModel userViewModel)
        {
            if (userViewModel != null)
            {
                ViewBag.ReturnURL = userViewModel.returnURL;
                return View(userViewModel);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id, string userRole, string returnURL)
        {
            if (string.IsNullOrEmpty(id)) return HttpNotFound();
            
            var user = await _userService.GetUserAsync(id);

            if(userRole == "SchoolAdmin")
            {
                var school = await _schoolService.GetSchoolAsync((int)user.SchoolId);

                school.SchoolAdminId = null;
                await _schoolService.UpdateSchoolAsync(school);
            }

            if(userRole == "Student")
            {
                await _userService.DeleteStudentAsync(id);
            }
            else
            {
                await _userService.DeleteUserAsync(id);
            }


            return Redirect(returnURL);
        }
    }
}