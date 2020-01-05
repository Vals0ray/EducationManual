using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EducationManual.Interfaces;
using EducationManual.Logs;
using EducationManual.Models;
using EducationManual.Services;
using EducationManual.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace EducationManual.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private IAuthenticationManager AuthenticationManager =>
            HttpContext.GetOwinContext().Authentication;

        private string UserIP => HttpContext.Request.UserHostAddress;

        private readonly IUserService _userService;
        private readonly IGenericService<School> _schoolService;

        public AccountController(IUserService userService, IGenericService<School> schoolService)
        {
            _userService = userService;
            _schoolService = schoolService;
        }

        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Register(int schoolId, int? classroomId = null, string role = null)
        {
            if (role != null)
            {
                ViewBag.SchoolId = schoolId;
                ViewBag.Role = role;
                ViewBag.ClassroomId = classroomId;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model, string role, int schoolId, int? classroomId = null)
        {
            if (ModelState.IsValid)
            {       
                IdentityResult result = null;

                ApplicationUser user = new ApplicationUser();

                user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    UserName = model.Email,
                    Email = model.Email,
                    SchoolId = schoolId
                };

                result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await UserManager.AddToRoleAsync(user.Id, role);

                    string message = $"[{UserIP}] [{User.Identity.Name}] registered new {role}: {user.UserName} in {DataSave.SchoolName}";
                    Logger.Log.Info(message);

                    if (role == "SchoolAdmin")
                    {
                        School school = _schoolService.Get(s => s.SchoolId == schoolId).First();

                        if(school.SchoolAdminId != null)
                        {
                            await UserManager.RemoveFromRoleAsync(school.SchoolAdminId, "SchoolAdmin");
                            await UserManager.AddToRolesAsync(school.SchoolAdminId, "Teacher");
                        }

                        school.SchoolAdminId = user.Id;
                        _schoolService.Update(school);

                        return RedirectToAction("Update", "School", new { id = schoolId });
                    }
                    else if (role == "Teacher")
                    {
                        return RedirectToAction("List", "User", new { usersRole = "Teacher" });
                    }
                    else if (role == "Student")
                    {
                        
                        Student student = new Student()
                        {
                            Id = user.Id,
                            ClassroomId = (int)classroomId
                        };

                        await _userService.AddStudentAsync(student);

                        return RedirectToAction("List", "User", new { usersRole = role, classroomId = classroomId });
                    }
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }

            ViewBag.ClassroomId = classroomId;
            ViewBag.SchoolId = schoolId;
            ViewBag.Role = role;
            return View(model);
        }

        public ActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);

                bool isBlocked = false;
                if (user != null)
                {
                    isBlocked = await UserManager.GetLockoutEnabledAsync(user.Id);
                }

                if (user == null)
                {
                    ModelState.AddModelError("", "Uncorrect login or password!");
                }
                else if (isBlocked)
                {
                    ModelState.AddModelError("", "Account has been blocking!");
                }
                else
                {
                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                            DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    string message = $"[{UserIP}] [{user.UserName}] conected!";
                    Logger.Log.Info(message); // Add sign in log
                    DataSave.Photo = null;

                    if (UserManager.IsInRole(user.Id, "SuperAdmin"))
                    {
                        return RedirectToAction("List", "School");
                    }
                    else if (UserManager.IsInRole(user.Id, "Student"))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("List", "Classroom", new { id = user.SchoolId });
                    }    
                }
            }
            return View(model);
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword(string id, string returnURl, string userName)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ViewBag.UserId = id;
                ViewBag.ReturnURL = returnURl;
                ViewBag.UserName = userName;
                return View();
            }

            return HttpNotFound();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model, string id, string returnURl)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.UserId = id;
                ViewBag.ReturnURL = returnURl;
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                return Redirect(returnURl);
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            ViewBag.UserId = id;
            ViewBag.ReturnURL = returnURl;
            return View(model);
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            string message = $"[{UserIP}] [{User.Identity.Name}] disconected!";
            Logger.Log.Info(message);
            return RedirectToAction("Login");
        }
    }
}