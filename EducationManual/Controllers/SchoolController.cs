using EducationManual.Models;
using EducationManual.ViewModels;
using EducationManual.Services;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace EducationManual.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class SchoolController : Controller
    {
        private ApplicationUserManager UserManager => 
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

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

                return RedirectToAction("List");
            }

            return HttpNotFound();
        }

        // Update existing school
        public async Task<ActionResult> Update(int? id)
        {
            if (id == null) return HttpNotFound();

            School school = await _schoolService.GetSchoolAsync((int)id);
            if(school != null)
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

            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Update(SchoolViewModel schoolViewModel)
        {
            if (schoolViewModel != null)
            {
                var school = await _schoolService.GetSchoolAsync(schoolViewModel.Id);
                if (school != null)
                {
                    school.Name = schoolViewModel.Name;

                    await _schoolService.UpdateSchoolAsync(school);

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
        public async Task<ActionResult> Delete(int? id)
        {
            if (id != null)
            {
                await _schoolService.DeleteSchoolAsync((int)id);

                return RedirectToAction("List");
            }

            return HttpNotFound();
        }

        // DeleteAdmin
        public async Task<ActionResult> DeleteAdmin(string userId)
        {
            var user = await UserManager.FindByIdAsync(userId);

            if (user != null) return View(user);

            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> DeleteAdmin(ApplicationUser modelUser)
        {
            var user = await UserManager.FindByIdAsync(modelUser.Id);
            var school = await _schoolService.GetSchoolAsync((int)user.SchoolId);

            school.SchoolAdminId = null;
            await _schoolService.UpdateSchoolAsync(school);

            await UserManager.RemoveFromRoleAsync(user.Id, "SchoolAdmin");
            await UserManager.AddToRolesAsync(user.Id, "Teacher");

            return RedirectToAction("Update", new { id = user.SchoolId });
        }
    }
}