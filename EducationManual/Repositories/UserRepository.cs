using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EducationManual.Models;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

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

        public async Task DeleteStudentAsync(int id)
        {
            using (var db = new ApplicationContext())
            {
                var student = await db.Students.FirstOrDefaultAsync(u => u.StudentId == id);

                db.Entry(student).State = EntityState.Deleted;

                await db.SaveChangesAsync();
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

        public async Task<Student> GetStudentAsync(int id)
        {
            Student result = null;

            using (var db = new ApplicationContext())
            {
                result = await db.Students.FirstOrDefaultAsync(s => s.StudentId == id);
            }

            return result;
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(int id)
        {
            var result = new List<Student>();

            using (var db = new ApplicationContext())
            {
                result = await db.Students.Include(s => s.Classroom)
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

        //public Task<ApplicationUser> GetUserByIdAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}


        //public Task<ApplicationUser> GetUserByRoleAsync(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}