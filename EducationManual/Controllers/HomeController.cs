using EducationManual.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using EducationManual.Interfaces;
using System.Linq;

namespace EducationManual.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationUserManager UserManager =>
            HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        private readonly IGenericService<School> _schoolService;

        public HomeController(IGenericService<School> schoolService)
        {
            _schoolService = schoolService;
        }

        public async Task<ActionResult> Index(string arg)
        {
            if (!User.IsInRole("SuperAdmin") && User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var currentUser = await UserManager.FindByIdAsync(userId);
                if (currentUser == null) return RedirectToAction("Logout", "Account");
                var school = _schoolService.Get(s => s.SchoolId == currentUser.SchoolId).First();

                DataSave.SchoolName = school.Name;
                DataSave.SchoolId = school.SchoolId; 
            }

            return View();
        }
    }
}