using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
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
                await UserManager.AddToRoleAsync(user.Id, role);

                if (result.Succeeded)
                {
                    if (role == "SchoolAdmin")
                    {
                        return RedirectToAction("Update", "School", new { id = schoolId, newSchoolAdminId = user.Id });
                    }
                    else if (role == "Teacher")
                    {
                        return RedirectToAction("List", "Teacher", new { id = schoolId, newSchoolAdminId = user.Id });
                    }
                    else if (role == "Student")
                    {
                        using (ApplicationContext db = new ApplicationContext())
                        {
                            Student student = new Student()
                            {
                                Id = user.Id,
                                ClassroomId = classroomId
                            };
                            db.Students.Add(student);
                            db.SaveChanges();
                        }
                        return RedirectToAction("List", "Student", new { id = classroomId });
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

            ViewBag.SchoolId = schoolId;
            ViewBag.Role = role;
            return View(model);
        }

        private IAuthenticationManager AuthenticationManager => 
            HttpContext.GetOwinContext().Authentication;

        public ActionResult Login(string returnUrl)
        {
            if (!User.Identity.IsAuthenticated)
            {
                ViewBag.returnUrl = returnUrl;
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
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

                    if (UserManager.IsInRole(user.Id, "SuperAdmin"))
                    {
                        return RedirectToAction("List", "School");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(returnUrl))
                        {
                            return RedirectToAction("List", "Classroom", new { id = user.SchoolId });
                        }
                    }
                    
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }
    }
}