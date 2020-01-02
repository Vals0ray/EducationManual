using EducationManual.Logs;
using EducationManual.Models;
using EducationManual.Services;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace EducationManual.Controllers
{
    [Authorize(Roles = "SuperAdmin, SchoolAdmin, Teacher")]
    public class ClassroomController : Controller
    {
        private string UserIP => HttpContext.Request.UserHostAddress;

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
            if (id != null)
            {
                var school = await _schoolService.GetSchoolAsync((int)id);
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
        public async Task<ActionResult> Create(Classroom classroom)
        {
            if (ModelState.IsValid)
            {
                classroom.SchoolId = DataSave.SchoolId;

                await _classroomService.AddClassroomAsync(classroom);

                string message = $"[{UserIP}] [{User.Identity.Name}] created classroom: " +
                                 $"{classroom.Name} in {DataSave.SchoolName}";
                Logger.Log.Info(message);

                return RedirectToAction("List", new { id = classroom.SchoolId });
            }

            return View("Create", classroom);
        }

        // Update existing Classroom
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Update(int? id)
        {
            if (id != null) 
            {
                var classroom = await _classroomService.GetClassroomAsync((int)id);
                if (classroom != null)
                    return View(classroom);
            }

            return HttpNotFound();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Update(Classroom newClassroom)
        {
            if (ModelState.IsValid)
            {
                var oldClassroom = await _classroomService.GetClassroomAsync(newClassroom.ClassroomId);
                if (oldClassroom != null)
                {
                    string message = $"[{UserIP}] [{User.Identity.Name}] changed classroom: " +
                                     $"{oldClassroom.Name} -> {newClassroom.Name} in {DataSave.SchoolName}";
                    Logger.Log.Info(message);

                    oldClassroom.Name = newClassroom.Name;
                    await _classroomService.UpdateClassroomAsync(oldClassroom);

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
        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var classroom = await _classroomService.GetClassroomAsync((int)id);
                if(classroom != null)
                {
                    await _classroomService.DeleteClassroomAsync((int)id);

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