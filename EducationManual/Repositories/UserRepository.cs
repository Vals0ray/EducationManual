using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EducationManual.Models;
using System.Data.Entity;

namespace EducationManual.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<Student> AddStudentAsync(Student student)
        {
            Student result = null;

            using (var db = new ApplicationContext())
            {
                result = db.Students.Add(student);
                await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task DeleteStudentAsync(string id)
        {
            using (var db = new ApplicationContext())
            {
                var student = await db.Students.Include(s => s.ApplicationUser)
                                               .FirstOrDefaultAsync(u => u.Id == id);

                db.Entry(student).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                await DeleteUserAsync(id);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            using (var db = new ApplicationContext())
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Id == id);

                db.Entry(user).State = EntityState.Deleted;

                await db.SaveChangesAsync();
            }
        }

        public async Task<Student> GetStudentAsync(string id)
        {
            Student result = null;

            using (var db = new ApplicationContext())
            {
                result = await db.Students.Include(s => s.ApplicationUser)
                                          .FirstOrDefaultAsync(s => s.Id == id);
            }

            return result;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(int id)
        {
            var result = new List<Student>();

            using (var db = new ApplicationContext())
            {
                result = await db.Students.Include(s => s.Classroom)
                                          .Include(s => s.ApplicationUser)
                                          .Where(s => s.ClassroomId == id)
                                          .ToListAsync();
            }

            return result;
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            ApplicationUser result = null;

            using (var db = new ApplicationContext())
            {
                result = await db.Users.Include(u => u.School)
                                       .FirstOrDefaultAsync(u => u.Id == id);
            }

            return result;
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            using (var db = new ApplicationContext())
            {
                db.Entry(student).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }

            return student;
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            using (var db = new ApplicationContext())
            {
                db.Entry(user).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }

            return user;
        }
    }
}