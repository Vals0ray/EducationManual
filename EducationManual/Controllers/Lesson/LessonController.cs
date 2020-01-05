using EducationManual.Models;
using EducationManual.ViewModels;
using EducationManual.Services;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EducationManual.Hubs;
using EducationManual.Interfaces;

namespace EducationManual.Controllers.Lesson
{
    [System.Web.Mvc.Authorize]
    public class LessonController : Controller
    {
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IGenericService<Classroom> _classroomService;

        private readonly IUserService _userService;

        private readonly ITaskService _taskService;

        public LessonController(IGenericService<Classroom> classroomService, IUserService userService, ITaskService taskService)
        {
            _classroomService = classroomService;
            _userService = userService;
            _taskService = taskService;
        }

        // Choose Classroom
        public async Task<ActionResult> ChooseClassroom()
        {
            string userId = User.Identity.GetUserId();
            var currentUser = await UserManager.FindByIdAsync(userId);
            int id = (int)currentUser.SchoolId;

            var classrooms = _classroomService.Get(c => c.ClassroomId == id).First();

            return View(classrooms);
        }

        // Choose Student
        private static List<LessonViewModel> lessonViewModels = new List<LessonViewModel>();

        public async Task<ActionResult> ChooseStudent(int classroomId, string studentId = null, int? testId = null)
        {
            var classroom = _classroomService.Get(c => c.ClassroomId == classroomId).First();
            ViewBag.ClassroomName = classroom.Name;
            ViewBag.ClassroomId = classroomId;
            var students = await _userService.GetStudentsAsync(classroomId);

            if(lessonViewModels.Count != students.ToList().Count)
            {
                lessonViewModels = new List<LessonViewModel>();

                foreach (var student in students)
                {
                    lessonViewModels.Add(new LessonViewModel() { Student = student });
                }
            }

            if(studentId != null)
            {
                lessonViewModels.First(l => l.Student.Id == studentId).TestId = testId;
            }

            return View(lessonViewModels);
        }

        // Choose Task
        public async Task<ActionResult> ChooseTask(string studentId, int classroomId)
        {
            // All testes with recomendation
            var tasks = await _taskService.GetTasks();

            ViewBag.StudentId = studentId;
            ViewBag.ClassroomId = classroomId;


            return View(tasks);
        }

        public void StartLesson()
        {
            SendMessage("Create LESSON!");
        }

        private void SendMessage(string message)
        {
            var context =
                Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

            foreach(var lesson in lessonViewModels)
            {
                var user = UserManager.FindById(lesson.Student.Id);
                context.Clients.User(user.Id).displayMessage(lesson.TestId.ToString());
            }
        }
    }
}