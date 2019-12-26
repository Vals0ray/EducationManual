using EducationManual.Logs;
using EducationManual.Models;
using EducationManual.Services;
using EducationManual.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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

        private string UserIP => HttpContext.Request.UserHostAddress;

        private readonly IUserService _userService;
        private readonly ISchoolService _schoolService;

        public UserController(IUserService userService, ISchoolService schoolService)
        {
            _userService = userService;
            _schoolService = schoolService;
        }

        public async Task<ActionResult> Details(string userId, string returnURL)
        {
            if (string.IsNullOrEmpty(userId)) 
                return HttpNotFound();

            var user = await UserManager.FindByIdAsync(userId);
            var userRole = (await UserManager.GetRolesAsync(user.Id)).First();
            var isBlocked = await UserManager.GetLockoutEnabledAsync(user.Id);
            var userSchoolName = (await _schoolService.GetSchoolAsync((int)user.SchoolId)).Name;

            UserViewModel userView = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = userRole,
                SchoolName = userSchoolName,
                ProfilePicture = user.ProfilePicture,
                isBlocked = isBlocked ? true : false
            };

            List<string> roles;
            if (userRole == "Student")
            {
                roles = new List<string>() { "Student" };
            }
            else if (User.IsInRole("Teacher"))
            {
                roles = new List<string>() { "Teacher" };
            }
            else if (User.IsInRole("SchoolAdmin"))
            {
                roles = new List<string>() { "SchoolAdmin", "Teacher" };
            }
            else
            {
                roles = new List<string>() { "SuperAdmin", "SchoolAdmin", "Teacher" };
            }

            var schools = await _schoolService.GetSchoolsAsync();
            IEnumerable<string> schoolNames;

            if (User.IsInRole("SuperAdmin"))
            {
                schoolNames = schools.Select(s => s.Name);
            }
            else
            {
                schoolNames = new List<string>() { userView.SchoolName };
            }

            ViewBag.Roles = new SelectList(roles, userView.Role);
            ViewBag.SchoolNames = new SelectList(schoolNames, userView.SchoolName);
            ViewBag.ReturnURL = returnURL;
            return View(userView);
        }

        [HttpPost]
        public async Task<ActionResult> Details(UserViewModel model, string returnURL, string returnImg)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            var userRole = (await UserManager.GetRolesAsync(model.Id)).First();

            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    var messages = CreateLog(user, model);
                    var school = await _schoolService.GetSchoolByNameAsync(model.SchoolName);

                    user.FirstName = model.FirstName;
                    user.SecondName = model.SecondName;
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.SchoolId = school.SchoolId;
                    if(returnImg != "null")
                    {
                        user.ProfilePicture = Encoding.ASCII.GetBytes(returnImg);
                    }

                    // If user role has changed
                    if (model.Role != userRole)
                    {
                        await ChangeRole(user.Id, userRole, model.Role);

                        // If we set new school admin, set new school link to admin
                        if (model.Role == "SchoolAdmin")
                        {
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
                            if (school.SchoolAdminId != null)
                            {
                                school.SchoolAdminId = null;
                                await _schoolService.UpdateSchoolAsync(school);
                            }
                        }
                    }

                    await Block(user.Id, model.isBlocked);

                    var result = await UserManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        foreach(var massege in messages)
                        {
                            Logger.Log.Info(massege);
                        }

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
            if (userRole == "Student")
            {
                roles = new List<string>() { "Student" };
            }
            else if (User.IsInRole("Teacher"))
            {
                roles = new List<string>() { "Teacher" };
            }
            else if (User.IsInRole("SchoolAdmin"))
            {
                roles = new List<string>() { "SchoolAdmin", "Teacher" };
            }
            else
            {
                roles = new List<string>() { "SuperAdmin", "SchoolAdmin", "Teacher" };
            }

            var schools = await _schoolService.GetSchoolsAsync();
            var schoolNames = schools.Select(s => s.Name);
            ViewBag.Roles = new SelectList(roles, model.Role);
            ViewBag.SchoolNames = new SelectList(schoolNames, model.SchoolName);
            ViewBag.ReturnURL = returnURL;
            return View(model);
        }

        private List<string> CreateLog(ApplicationUser oldUser, UserViewModel newUser)
        {
            string pattern = $"[{UserIP}] [{User.Identity.Name}]";
            List<string> messages = new List<string>();

            if(oldUser.FirstName != newUser.FirstName)
            {
                messages.Add($"{pattern} changed [{oldUser.Email}] First name: {oldUser.FirstName} -> {newUser.FirstName}");
            }

            if (oldUser.SecondName != newUser.SecondName)
            {
                messages.Add($"{pattern} changed [{oldUser.Email}] Second name: {oldUser.SecondName} -> {newUser.SecondName}");
            }

            if (oldUser.Email != newUser.Email)
            {
                messages.Add($"{pattern} changed [{oldUser.Email}] E-mail: {oldUser.Email} -> {newUser.Email}");
            }

            if(oldUser.PhoneNumber != newUser.PhoneNumber)
            {
                messages.Add($"{pattern} changed [{oldUser.Email}] Phone number: {oldUser.PhoneNumber} -> {newUser.PhoneNumber}");
            }

            //if (oldUser.School.Name != newUser.SchoolName)
            //{
            //    message = $"{pattern} changed user school {oldUser.Email}: {oldUser.School.Name} -> {newUser.SchoolName}";
            //    Logger.Log.Info(message);
            //}

            return messages;
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
            if(string.IsNullOrEmpty(usersRole)) 
                return HttpNotFound();

            // Look up the role
            var role = RoleManager.Roles.Single(r => r.Name == usersRole);

            // Find the users in that role
            var roleUsers = UserManager.Users
                .Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                .Include(u => u.School);

            if(DataSave.SchoolName != "")
            {
                roleUsers = roleUsers.Where(u => u.SchoolId == DataSave.SchoolId);
            }

            ViewBag.UsersRole = usersRole;
            return View(roleUsers.ToList());
        }

        public async Task<ActionResult> Delete(string id, string userRole, string returnUrl, string adssa)
        {
            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);

                UserViewModel userView = new UserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = userRole,
                    ProfilePicture = user.ProfilePicture
                };

                ViewBag.ReturnURL = returnUrl;
                return View(userView);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id, string userRole, string returnURL)
        {
            if (string.IsNullOrEmpty(id)) 
                return HttpNotFound();
            
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

            string pattern = $"[{UserIP}] [{User.Identity.Name}]";
            string message = $"{pattern} deleted [{user.Email}]";
            Logger.Log.Info(message);

            return Redirect(returnURL);
        }
    }
}