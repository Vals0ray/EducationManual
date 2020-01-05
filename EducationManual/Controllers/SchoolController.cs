using EducationManual.Models;
using EducationManual.ViewModels;
using EducationManual.Interfaces;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using EducationManual.Logs;
using System.Linq;

namespace EducationManual.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SchoolController : Controller
    {
        private ApplicationUserManager UserManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private string UserIP => HttpContext.Request.UserHostAddress;

        private readonly IGenericService<School> _schoolService;

        public SchoolController(IGenericService<School> schoolService)
        {
            _schoolService = schoolService;
        }

        // Out put list of schools
        public ActionResult List()
        {
            var schools = _schoolService
                .GetWithInclude(s => s.ApplicationUsers, s => s.Classrooms.Select(c => c.Students));

            if(schools != null)
            {
                DataSave.SchoolName = "";
                return View(schools);
            }
            
            return HttpNotFound();
        }

        // Create new school
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(SchoolViewModel schoolViewModel)
        {
            if (ModelState.IsValid) 
            {
                var newSchool = new School() { Name = schoolViewModel.Name };

                _schoolService.Create(newSchool);

                string message = $"[{UserIP}] [{User.Identity.Name}] created school: {newSchool.Name}";
                Logger.Log.Info(message);

                return RedirectToAction("List");
            }

            return View("Create");
        }

        // Update existing school
        public async Task<ActionResult> Update(int? id)
        {
            if (id != null)
            {
                School school = _schoolService.Get(s => s.SchoolId == id).First();
                if (school != null)
                {
                    SchoolViewModel schoolViewModel = new SchoolViewModel()
                    {
                        Id = school.SchoolId,
                        Name = school.Name,
                        SchoolAdmin = school.SchoolAdminId == null ?
                            null : await UserManager.FindByIdAsync(school.SchoolAdminId)
                    };

                    return View(schoolViewModel);
                }
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Update(SchoolViewModel newSchool)
        {
            if (ModelState.IsValid)
            {
                var oldSchool = _schoolService.Get(s => s.SchoolId == newSchool.Id).First();
                if (oldSchool != null)
                {
                    string message = $"[{UserIP}] [{User.Identity.Name}] changed school: " +
                                     $"{oldSchool.Name} -> {newSchool.Name}";
                    Logger.Log.Info(message);

                    oldSchool.Name = newSchool.Name;
                    _schoolService.Update(oldSchool);

                    return RedirectToAction("List");
                }
            }

            return View("Update", newSchool);
        }

        // Delete existing school
        public ActionResult Delete(SchoolViewModel schoolViewModel)
        {
            if(schoolViewModel != null) return View(schoolViewModel);

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult Delete(int? id, string schoolName)
        {
            if (id != null && !string.IsNullOrEmpty(schoolName))
            {
                var school = _schoolService.Get(s => s.SchoolId == id).First();
                _schoolService.Remove(school);

                string message = $"[{UserIP}] [{User.Identity.Name}] deleted school: {schoolName}";
                Logger.Log.Info(message);

                return RedirectToAction("List");
            }

            return HttpNotFound();
        }
    }
}