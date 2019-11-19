using Microsoft.Owin;
using Owin;
using EducationManual.Models;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using EducationManual.Hubs;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(EducationManual.App_Start.Startup))]

namespace EducationManual.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var idProvider = new CustomUserIdProvider();

            GlobalHost.DependencyResolver.Register(typeof(IUserIdProvider), () => idProvider);

            app.CreatePerOwinContext<ApplicationContext>(ApplicationContext.Create);

            //ApplicationContext db = new ApplicationContext();
            //db.Database.Delete();
            //db.Database.Create();

            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);

            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });

            app.MapSignalR();
        }
    }
}