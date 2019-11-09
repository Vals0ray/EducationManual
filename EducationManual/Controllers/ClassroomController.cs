﻿using EducationManual.Models;
using EducationManual.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EducationManual.Controllers
{
    [Authorize]
    public class ClassroomController : Controller
    {
        private ApplicationUserManager UserManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IClassroomService _classroomService;

        private readonly ISchoolService _schoolService;

        public ClassroomController(IClassroomService classroomService, ISchoolService schoolService)
        {
            _classroomService = classroomService;
            _schoolService = schoolService;
        }

        // Out put list of Classrooms
        public async Task<ActionResult> List(int? id)
        {
            if (id == null)
            {
                string userId = User.Identity.GetUserId();
                var currentUser = await UserManager.FindByIdAsync(userId);
                id = currentUser.SchoolId;
                if (User.IsInRole("SuperAdmin")) id = DataSave.SchoolId;
            }

            var classrooms = await _classroomService.GetClassroomsAsync((int)id);
            var school = await _schoolService.GetSchoolAsync((int)id);

            ViewBag.SchoolId = school.SchoolId;
            DataSave.SchoolName = school.Name;
            DataSave.SchoolId = school.SchoolId;

            return View(classrooms);
        }

        // Create new Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Create(int schoolId)
        {
            ViewBag.SchoolId = schoolId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Create(Classroom classroom)
        {
            if (classroom != null)
            {
                await _classroomService.AddClassroomAsync(classroom);

                return RedirectToAction("List", new { id = classroom.SchoolId});
            }

            return HttpNotFound();
        }

        // Update existing Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null) return HttpNotFound();

            var classroom = await _classroomService.GetClassroomAsync((int)id);

            if (classroom != null)
            {
                return View(classroom);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Update(Classroom classroom)
        {
            if (classroom != null)
            {
                var result = await _classroomService.GetClassroomAsync(classroom.ClassroomId);
                if (result != null)
                {
                    result.Name = classroom.Name;

                    await _classroomService.UpdateClassroomAsync(result);

                    return RedirectToAction("List", new { id = result.SchoolId });
                }
            }

            return HttpNotFound();
        }

        // Delete existing Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Delete(Classroom classroom)
        {
            if (classroom == null) return HttpNotFound();

            return View(classroom);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return HttpNotFound();

            var classroom = await _classroomService.GetClassroomAsync((int)id);

            await _classroomService.DeleteClassroomAsync((int)id);

            return RedirectToAction("List", new { id = classroom.SchoolId });
        }
    }
}