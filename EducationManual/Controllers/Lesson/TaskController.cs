using System.Web.Mvc;

namespace EducationManual.Controllers.Lesson
{
    public class TaskController : Controller
    {
        public string Index(int taskId)
        {
            return $"Start! {taskId}";
        }
    }
}