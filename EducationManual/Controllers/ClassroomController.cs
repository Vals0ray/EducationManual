using EducationManual.Interfaces;
using EducationManual.Logs;
using EducationManual.Models;
using System.Linq;
using System.Web.Mvc;

namespace EducationManual.Controllers
{
    [Authorize(Roles = "SuperAdmin, SchoolAdmin, Teacher")]
    public class ClassroomController : Controller
    {
        private string UserIP => HttpContext.Request.UserHostAddress;

        private readonly IGenericService<Classroom> _classroomService;
        private readonly IGenericService<School> _schoolService;

        public ClassroomController(IGenericService<Classroom> classroomService, IGenericService<School> schoolService)
        {
            _classroomService = classroomService;
            _schoolService = schoolService;
        }

        // Out put list of Classrooms
        public ActionResult List(int? id)
        {
            if (id != null)
            {
                var school = _schoolService.GetWithInclude(s => s.SchoolId == id, s => s.ApplicationUsers, s => s.Classrooms.Select(c => c.Students)).First();
                if (school != null)
                {
                    DataSave.SchoolName = school.Name;
                    DataSave.SchoolId = school.SchoolId;

                    return View(school);
                }
            }

            return HttpNotFound();
        }

        // Create new Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                classroom.SchoolId = (int)DataSave.SchoolId;

                _classroomService.Create(classroom);

                string message = $"[{UserIP}] [{User.Identity.Name}] created classroom: " +
                                 $"{classroom.Name} in {DataSave.SchoolName}";
                Logger.Log.Info(message);

                return RedirectToAction("List", new { id = classroom.SchoolId });
            }

            return View("Create", classroom);
        }

        // Update existing Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Update(int? id)
        {
            if (id != null) 
            {
                var classroom = _classroomService.Get(c => c.ClassroomId == id).First();
                if (classroom != null)
                    return View(classroom);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Update(Classroom newClassroom)
        {
            if (ModelState.IsValid)
            {
                var oldClassroom = _classroomService.Get(c => c.ClassroomId == newClassroom.ClassroomId).First();
                if (oldClassroom != null)
                {
                    string message = $"[{UserIP}] [{User.Identity.Name}] changed classroom: " +
                                     $"{oldClassroom.Name} -> {newClassroom.Name} in {DataSave.SchoolName}";
                    Logger.Log.Info(message);

                    oldClassroom.Name = newClassroom.Name;
                    _classroomService.Update(oldClassroom);

                    return RedirectToAction("List", new { id = oldClassroom.SchoolId });
                }
            }

            return View("Update", newClassroom);
        }

        // Delete existing Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Delete(Classroom classroom)
        {
            if (classroom != null)
                return View(classroom);

            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public ActionResult Delete(int? id)
        {
            if (id != null)
            {
                var classroom = _classroomService.Get(c => c.ClassroomId == id).First();
                if(classroom != null)
                {
                    _classroomService.Remove(classroom);

                    string message = $"[{UserIP}] [{User.Identity.Name}] deleted classroom: " +
                                     $"{classroom.Name} in {DataSave.SchoolName}";
                    Logger.Log.Info(message);

                    return RedirectToAction("List", new { id = classroom.SchoolId });
                }
            }

            return HttpNotFound();
        }
    }
}