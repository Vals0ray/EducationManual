using System.Web.Mvc;

namespace EducationManual.Controllers.Lesson
{
    public class TaskController : Controller
    {
        public ViewResult Index(int taskId)
        {
            ViewBag.Message = $"Start! {taskId}"; ;
            return View(); 
        }
    }
}