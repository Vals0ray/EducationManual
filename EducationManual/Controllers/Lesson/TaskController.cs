using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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