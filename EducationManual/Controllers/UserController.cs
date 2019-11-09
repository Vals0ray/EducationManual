using EducationManual.Models;
using EducationManual.Services;
using EducationManual.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace EducationManual.Controllers
{
    //[Authorize(Roles = "SuperAdmin, SchoolAdmin")]
    public class UserController : Controller
    {
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<ActionResult> Details(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);
            var userRole = await UserManager.GetRolesAsync(user.Id);
            var isBlocked = await UserManager.GetLockoutEnabledAsync(user.Id);

            UserViewModel userView = new UserViewModel()
            {
                Id = user.Id,
                FirstName = user.UserName,
                SecondName = null,
                Role = userRole.First(),
                SchoolId = user.SchoolId,
                isBlocked = isBlocked ? true : false
            };

            return View(userView);
        }

        [HttpPost]
        public async Task<ActionResult> Details(UserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            var userRole = await UserManager.GetRolesAsync(model.Id);
            var isBlocked = await UserManager.GetLockoutEnabledAsync(model.Id);

            if (user != null)
            {
                user.UserName = model.FirstName;
                if (model.Role != userRole.First())
                {
                    await UserManager.RemoveFromRoleAsync(user.Id, userRole.First());
                    await UserManager.AddToRolesAsync(user.Id, model.Role);
                }
                user.SchoolId = model.SchoolId;
                await Block(user.Id, model.isBlocked);

                await UserManager.UpdateAsync(user);

                return RedirectToAction("Update", "School", new { id = user.SchoolId });
            }

            return HttpNotFound();
        }

        public async Task Block(string userId, bool isBlocked)
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
    }
}