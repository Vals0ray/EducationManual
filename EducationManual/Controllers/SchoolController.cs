using EducationManual.Models;
using EducationManual.ViewModels;
using EducationManual.Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using EducationManual.Logs;

namespace EducationManual.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SchoolController : Controller
    {
        private ApplicationUserManager UserManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private string UserIP => HttpContext.Request.UserHostAddress;

        private readonly ISchoolService _schoolService;

        public SchoolController(ISchoolService schoolService)
        {
            _schoolService = schoolService;
        }

        // Out put list of schools
        public async Task<ActionResult> List()
        {
            var schools = await _schoolService.GetSchoolsAsync();
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
        public async Task<ActionResult> Create(SchoolViewModel schoolViewModel)
        {
            if (schoolViewModel != null) 
            {
                var newSchool = new School() { Name = schoolViewModel.Name };

                await _schoolService.AddSchoolAsync(newSchool);

                string message = $"[{UserIP}] [{User.Identity.Name}] created school: {newSchool.Name}";
                Logger.Log.Info(message);

                return RedirectToAction("List");
            }

            return HttpNotFound();
        }

        // Update existing school
        public async Task<ActionResult> Update(int? id)
        {
            if (id != null)
            {
                School school = await _schoolService.GetSchoolAsync((int)id);
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
        public async Task<ActionResult> Update(SchoolViewModel newSchool)
        {
            if (newSchool != null)
            {
                var oldSchool = await _schoolService.GetSchoolAsync(newSchool.Id);
                if (oldSchool != null)
                {
                    string message = $"[{UserIP}] [{User.Identity.Name}] changed school: " +
                                     $"{oldSchool.Name} -> {newSchool.Name}";
                    Logger.Log.Info(message);

                    oldSchool.Name = newSchool.Name;
                    await _schoolService.UpdateSchoolAsync(oldSchool);

                    return RedirectToAction("List");
                }
            }

            return HttpNotFound();
        }

        // Delete existing school
        public ActionResult Delete(SchoolViewModel schoolViewModel)
        {
            if(schoolViewModel != null) return View(schoolViewModel);

            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int? id, string schoolName)
        {
            if (id != null && !string.IsNullOrEmpty(schoolName))
            {
                await _schoolService.DeleteSchoolAsync((int)id);

                string message = $"[{UserIP}] [{User.Identity.Name}] deleted school: {schoolName}";
                Logger.Log.Info(message);

                return RedirectToAction("List");
            }

            return HttpNotFound();
        }
    }
}