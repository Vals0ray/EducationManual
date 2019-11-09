using EducationManual.Models;
using EducationManual.ViewModels;
using EducationManual.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace EducationManual.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private readonly IUserService _userService;

        private readonly IClassroomService _classroomService;

        public StudentController(IUserService userService, IClassroomService classroomService)
        {
            _userService = userService;
            _classroomService = classroomService;
        }

        public async Task<ActionResult> List(int id)
        {
            var classroom = await _classroomService.GetClassroomAsync(id);
            ViewBag.ClassroomName = classroom.Name;
            ViewBag.ClassroomId = id;
            var students = await _userService.GetStudentsAsync(id);

            return View(students);
        }

        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null) return HttpNotFound();

            var student = await _userService.GetStudentAsync((int)id);

            if (student != null)
            {
                return View(student);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Update(Student student)
        {
            if (student != null)
            {
                var result = await _userService.GetStudentAsync(student.StudentId);
                if (result != null)
                {
                    result.FirstName = student.FirstName;
                    result.SecondName = student.SecondName;

                    await _userService.UpdateStudentAsync(result);

                    return RedirectToAction("List", new { id = result.ClassroomId });
                }
            }

            return HttpNotFound();
        }

        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Create(int classroomId)
        {
            ViewBag.ClassroomId = classroomId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Create(StudentViewModel model, int classroomId)
        {
            Student student = new Student()
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ClassroomId = classroomId
            };

            await _userService.AddStudentAsync(student);

            return RedirectToAction("List", new { id = classroomId });
        }

        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Delete(Student student)
        {
            if (student == null) return HttpNotFound();

            return View(student);
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null) return HttpNotFound();

            Student student = await _userService.GetStudentAsync((int)id);

            await _userService.DeleteStudentAsync((int)id);

            return RedirectToAction("List", new { id = student.ClassroomId });
        }
    }
}