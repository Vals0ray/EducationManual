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
    }
}