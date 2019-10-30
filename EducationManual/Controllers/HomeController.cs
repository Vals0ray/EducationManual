using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EducationManual.Models;

namespace EducationManual.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationContext db = new ApplicationContext();

        public List<Classroom> _Classrooms { get; set; }

        public ActionResult Index(string arg)
        {
            return View();
        }

        [Authorize]
        public ActionResult Classrooms()
        {
            _Classrooms = db.Classrooms.ToList();

            foreach (var cr in _Classrooms)
            {
                cr.Students = db.Students.Where(s => s.ClassroomId == cr.Id).ToList();
            }

            return View(_Classrooms);
        }

        [Authorize]
        public ActionResult Students(int id)
        {
            _Classrooms = db.Classrooms.ToList();

            foreach (var cr in _Classrooms)
            {
                cr.Students = db.Students.Where(s => s.ClassroomId == cr.Id).ToList();
            }

            var students = _Classrooms.FirstOrDefault(c => c.Id == id).Students;

            return View(students.ToList());
        }
    }
}