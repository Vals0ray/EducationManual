using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationManual.Models;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using EducationManual.Interfaces;

namespace EducationManual.Repositories
{
    public class UserRepository : IUserRepository
    {
        public ApplicationContext Database { get; set; }
        public UserRepository(ApplicationContext db)
        {
            Database = db;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            Student result = null;

            result = Database.Students.Add(student);
            await Database.SaveChangesAsync();

            return result;
        }

        public async Task DeleteStudentAsync(string id)
        {
            var student = await Database.Students.Include(s => s.ApplicationUser)
                                            .FirstOrDefaultAsync(u => u.Id == id);

            if(student != null)
            {
                Database.Entry(student).State = EntityState.Deleted;

                await Database.SaveChangesAsync();

                await DeleteUserAsync(id);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await Database.Users.FirstOrDefaultAsync(u => u.Id == id);

            Database.Entry(user).State = EntityState.Deleted;

            await Database.SaveChangesAsync();
        }

        public async Task<Student> GetStudentAsync(string id)
        {
            Student result = null;

            result = await Database.Students.Include(s => s.ApplicationUser)
                                        .FirstOrDefaultAsync(s => s.Id == id);

            return result;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(int id)
        {
            var result = new List<Student>();

            result = await Database.Students.Include(s => s.Classroom)
                                        .Include(s => s.ApplicationUser)
                                        .Where(s => s.ClassroomId == id)
                                        .ToListAsync();

            return result;
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            ApplicationUser result = null;

            result = await Database.Users.Include(u => u.School)
                                    .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }

        public async Task<ApplicationUser> GetUserWithoutTrackingAsync(string id)
        {
            ApplicationUser result = null;

            result = await Database.Users.AsNoTracking()
                                    .Include(u => u.School)
                                    .FirstOrDefaultAsync(u => u.Id == id);

            return result;
        }

        public async Task<IEnumerable<ApplicationUser>> GetUserByRoleAsync(string usersRole)
        {
            var result = new List<ApplicationUser>();
            var RoleManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>();

            // Look up the role
            var role = RoleManager.Roles.Single(r => r.Name == usersRole);

            // Find the users in that role
            result = await Database.Users.Where(u => u.Roles.Any(r => r.RoleId == role.Id))
                                    .Include(u => u.School)
                                    .ToListAsync();

            return result;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            Database.Entry(student).State = EntityState.Modified;

            await Database.SaveChangesAsync();

            return student;
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            IdentityResult user1 = null;

            var upUser = new ApplicationUser()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                SecondName = user.SecondName,
                Email = user.Email,
                UserName = user.Email,
                PhoneNumber = user.PhoneNumber,
                SchoolId = user.SchoolId,
                ProfilePicture = user.ProfilePicture,
                LockoutEnabled = user.LockoutEnabled,
                PasswordHash = user.PasswordHash,
                SecurityStamp = user.SecurityStamp
            };

            using (var db = new ApplicationContext())
            {
                var _userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                db.Users.Attach(upUser);
                user1 = await _userManager.UpdateAsync(upUser);
            }

            return user1;
        }
    }
}