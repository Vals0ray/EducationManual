using EducationManual.Models;
using EducationManual.Services;
using System.Threading.Tasks;
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
        public ActionResult Create(int classroomId)
        {
            ViewBag.ClassroomId = classroomId;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin, SchoolAdmin")]
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null) return HttpNotFound();

            Student student = await _userService.GetStudentAsync(id);

            await _userService.DeleteStudentAsync(id);

            return RedirectToAction("List", new { id = student.ClassroomId });
        }
    }
}