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
            
            DataSave.SchoolName = "";
            return View(schools);
        }

        // Create new school
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(School school)
        {
            if (school != null) 
            {
                await _schoolService.AddSchoolAsync(school);

                return RedirectToAction("List");
            }

            return HttpNotFound();
        }

        // Update existing school
        public async Task<ActionResult> Update(int id, string newSchoolAdminId = null)
        {
            School school = await _schoolService.GetSchoolAsync(id);

            SchoolViewModel schoolViewModel = new SchoolViewModel()
            {
                Id = school.SchoolId,
                Name = school.Name,
                SchoolAdmin =
                school.SchoolAdminId == null ? 
                null : await UserManager.FindByIdAsync(school.SchoolAdminId)
            };

            if(school != null)
            {
                if(newSchoolAdminId != null)
                {
                    if(school.SchoolAdminId != null)
                    {
                        await UserManager.RemoveFromRoleAsync(schoolViewModel.SchoolAdmin.Id, "SchoolAdmin");
                        await UserManager.AddToRolesAsync(schoolViewModel.SchoolAdmin.Id, "Teacher");
                    }

                    schoolViewModel.SchoolAdmin = await UserManager.FindByIdAsync(newSchoolAdminId);
                }

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
                    if(schoolViewModel.SchoolAdmin != null)
                    {
                        school.SchoolAdminId = schoolViewModel.SchoolAdmin.Id;
                    }

                    await _schoolService.UpdateSchoolAsync(school);

                    return RedirectToAction("List");
                }
            }

            return HttpNotFound();
        }

        // Delete existing school
        public ActionResult Delete(School school)
        {
            if(school != null) return View(school);

            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            await _schoolService.DeleteSchoolAsync(id);
            
            return RedirectToAction("List");
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