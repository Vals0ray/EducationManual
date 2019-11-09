﻿using EducationManual.Models;
using EducationManual.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EducationManual.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly ISchoolService _schoolService;

        public HomeController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        public async Task<ActionResult> Index(string arg)
        {
            if (!User.IsInRole("SuperAdmin"))
            {
                var userId = User.Identity.GetUserId();
                var currentUser = await UserManager.FindByIdAsync(userId);
                var school = await _schoolService.GetSchoolAsync((int)currentUser.SchoolId);

                DataSave.SchoolName = school.Name;
                DataSave.SchoolId = school.SchoolId;
            }

            return View();
        }
    }
}