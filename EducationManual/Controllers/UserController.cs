using EducationManual.Logs;
using EducationManual.Models;
using EducationManual.Services;
using EducationManual.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
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

        private string UserIP => HttpContext.Request.UserHostAddress;

        private readonly IUserService _userService;
        private readonly ISchoolService _schoolService;

        public UserController(IUserService userService, ISchoolService schoolService)
        {
            _userService = userService;
            _schoolService = schoolService;
        }

        // Display list of users in specific role
        [HttpGet]
        public async Task<ActionResult> List(string usersRole, int? classroomId)
        {
            if (!string.IsNullOrEmpty(usersRole))
            {
                var usersInRole = await _userService.GetUserByRoleAsync(usersRole);
                if (usersInRole != null)
                {
                    if (DataSave.SchoolName != "")
                    {
                        usersInRole = usersInRole.Where(u => u.SchoolId == DataSave.SchoolId);
                    }

                    if(classroomId != null)
                    {
                        var students = await _userService.GetStudentsAsync((int)classroomId);
                        if(students != null)
                        {
                            var studentsUsers = students.Select(s => s.ApplicationUser).ToList();

                            usersInRole = usersInRole.Where(u => studentsUsers.Any(s => s.Id == u.Id));

                            ViewBag.ClassroomId = classroomId;
                        }
                    }

                    var usersViewModel = new List<UserViewModel>();
                    foreach (var userInRole in usersInRole)
                    {
                        var userViewModel = new UserViewModel()
                        {
                            Id = userInRole.Id,
                            FirstName = userInRole.FirstName,
                            SecondName = userInRole.SecondName,
                            Email = userInRole.Email,
                            PhoneNumber = userInRole.PhoneNumber,
                            SchoolName =  userInRole.School.Name
                        };

                        usersViewModel.Add(userViewModel);
                    }

                    ViewBag.UsersRole = usersRole;
                    return View(usersViewModel);
                }
            }
               
            return HttpNotFound();
        }

        // Get details information about selected user
        [HttpGet]
        public async Task<ActionResult> Details(string userId, string returnURL)
        {
            if (string.IsNullOrEmpty(userId)) 
                return HttpNotFound();

            var user = await _userService.GetUserWithoutTrackingAsync(userId);
            if (user != null) 
            {
                var userRoles = await UserManager.GetRolesAsync(user.Id);

                UserViewModel userView = new UserViewModel()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    SecondName = user.SecondName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Role = userRoles.First(),
                    SchoolId = (int)user.SchoolId,
                    SchoolName = user.School.Name,
                    ProfilePicture = user.ProfilePicture is null ? null : Encoding.ASCII.GetString(user.ProfilePicture),
                    isBlocked = user.LockoutEnabled,
                    returnURL = returnURL
                };

                await CreateSchoolsAndRolesViews(userView);

                return View(userView);
            }

            return HttpNotFound();
        }

        // Cheack and save changes in selected user 
        [HttpPost]
        public async Task<ActionResult> Details(UserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                var oldUser = await _userService.GetUserAsync(newUser.Id);
                var userRole = (await UserManager.GetRolesAsync(newUser.Id)).First();
                var school = await _schoolService.GetSchoolByNameAsync(newUser.SchoolName);

                if (oldUser != null)
                {
                    UpdateUserAndCreateLog(oldUser, newUser);
                    oldUser.SchoolId = school.SchoolId;

                    // If user role has changed
                    if (newUser.Role != userRole)
                    {
                        await ChangeRole(oldUser.Id, userRole, newUser.Role);

                        // If we set new school admin, set new school link to admin
                        if (newUser.Role == "SchoolAdmin")
                        {
                            // Change old school admin to teacher
                            if (school.SchoolAdminId != null)
                            {
                                var oldSchoolAdmin = await UserManager.FindByIdAsync(school.SchoolAdminId);
                                await ChangeRole(oldSchoolAdmin.Id, "SchoolAdmin", "Teacher");
                                await UserManager.UpdateAsync(oldSchoolAdmin);
                            }

                            school.SchoolAdminId = oldUser.Id;
                            await _schoolService.UpdateSchoolAsync(school);
                        }
                        // If we change school admin role, delete in school link to admin
                        else if (newUser.Role != "SchoolAdmin" && userRole == "SchoolAdmin")
                        {
                            if (school.SchoolAdminId != null)
                            {
                                school.SchoolAdminId = null;
                                await _schoolService.UpdateSchoolAsync(school);
                            }
                        }
                    }

                    var result = await _userService.UpdateUserAsync(oldUser);
                    if (result.Succeeded)
                    {
                        return Redirect(newUser.returnURL);
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

            await CreateSchoolsAndRolesViews(newUser);
            
            return View(newUser);
        }

        private async Task CreateSchoolsAndRolesViews(UserViewModel userView)
        {
            var schools = await _schoolService.GetSchoolsAsync();
            var schoolsNames = new List<string> { userView.SchoolName };
            var roles = new List<string>();

            if (userView.Role.Contains("Student"))
            {
                roles.Add("Student");
            }
            else if (User.IsInRole("Teacher"))
            {
                roles.Add("Teacher");
            }
            else if (User.IsInRole("SchoolAdmin"))
            {
                roles.Add("SchoolAdmin");
                roles.Add("Teacher");
            }
            else if (User.IsInRole("SuperAdmin"))
            {
                schoolsNames = schools.Select(s => s.Name).ToList();
                roles.Add("SuperAdmin");
                roles.Add("SchoolAdmin");
                roles.Add("Teacher");
            }

            ViewBag.Roles = new SelectList(roles, userView.Role);
            ViewBag.SchoolNames = new SelectList(schoolsNames, userView.SchoolName);
        }

        private void UpdateUserAndCreateLog(ApplicationUser oldUser, UserViewModel newUser)
        {
            string pattern = $"[{UserIP}] [{User.Identity.Name}] changed [{oldUser.Email}]";
            string message;

            if (oldUser.FirstName != newUser.FirstName)
            {
                message = $"{pattern} ftname: {oldUser.FirstName} -> {newUser.FirstName}";
                Logger.UserUpdateLog(message);
                oldUser.FirstName = newUser.FirstName;
            }

            if (oldUser.SecondName != newUser.SecondName)
            {
                message = $"{pattern} sdname: {oldUser.SecondName} -> {newUser.SecondName}";
                Logger.UserUpdateLog(message);
                oldUser.SecondName = newUser.SecondName;
            }

            if (oldUser.Email != newUser.Email)
            {
                message = $"{pattern} e-mail: {oldUser.Email} -> {newUser.Email}";
                Logger.UserUpdateLog(message);
                oldUser.Email = oldUser.UserName = newUser.Email;
            }

            if(oldUser.PhoneNumber != newUser.PhoneNumber)
            {
                message = $"{pattern} number: {oldUser.PhoneNumber} -> {newUser.PhoneNumber}";
                Logger.UserUpdateLog(message);
                oldUser.PhoneNumber = newUser.PhoneNumber;
            }

            if (oldUser.School.Name != newUser.SchoolName)
            {
                message = $"{pattern} school: {oldUser.School.Name} -> {newUser.SchoolName}";
                Logger.UserUpdateLog(message);
            }

            if (oldUser.LockoutEnabled != newUser.isBlocked)
            {
                message = $"{pattern} locked: {oldUser.LockoutEnabled} -> {newUser.isBlocked}";
                Logger.UserUpdateLog(message);
                oldUser.LockoutEnabled = newUser.isBlocked;
            }

            if (newUser.ProfilePicture != null)
            {
                var newUserPicture = Encoding.ASCII.GetBytes(newUser.ProfilePicture);
                if (oldUser.ProfilePicture == null || !Enumerable.SequenceEqual(oldUser.ProfilePicture, newUserPicture))
                {
                    message = $"{pattern} photo!";
                    Logger.UserUpdateLog(message);
                    oldUser.ProfilePicture = newUserPicture;
                }
            }
        }

        private async Task ChangeRole(string userId, string fromRole, string toRole)
        {
            await UserManager.RemoveFromRoleAsync(userId, fromRole);
            await UserManager.AddToRolesAsync(userId, toRole);
        }

        // Ask whether or not to remove selected the user
        [HttpGet]
        public async Task<ActionResult> Delete(string id, string userRole)
        {
            if (id != null && userRole != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    UserViewModel userView = new UserViewModel()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        SecondName = user.SecondName,
                        SchoolId = (int)user.SchoolId,
                        Email = user.Email,
                        Role = userRole
                    };

                    return View(userView);
                }
            }

            return HttpNotFound();
        }

        // Remove selected user
        [HttpPost]
        public async Task<ActionResult> Delete(UserViewModel userViewModel)
        {
            if (userViewModel == null) 
                return HttpNotFound();

            switch (userViewModel.Role)
            {
                case "SchoolAdmin":
                    var school = await _schoolService.GetSchoolAsync(userViewModel.SchoolId);
                    school.SchoolAdminId = null;
                    await _schoolService.UpdateSchoolAsync(school);
                    await _userService.DeleteUserAsync(userViewModel.Id);
                    break;
                case "Student":
                    await _userService.DeleteStudentAsync(userViewModel.Id);
                    break;
                default:
                    await _userService.DeleteUserAsync(userViewModel.Id);
                    break;
            }

            string message = $"[{UserIP}] [{User.Identity.Name}] delete [{userViewModel.Email}]";
            Logger.UserUpdateLog(message);

            return Redirect(Request.UrlReferrer.AbsoluteUri);
        }
    }
}