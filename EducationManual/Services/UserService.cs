using EducationManual.Interfaces;
using EducationManual.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public class UserService : IUserService
    {
        private IUnitOfWork Database { get; set; }

        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            return await Database.UserManager.AddStudentAsync(student);
        }

        public async Task DeleteStudentAsync(string id)
        {
            await Database.UserManager.DeleteStudentAsync(id);
        }

        public async Task DeleteUserAsync(string id)
        {
            await Database.UserManager.DeleteUserAsync(id);
        }

        public async Task<Student> GetStudentAsync(string id)
        {
            return await Database.UserManager.GetStudentAsync(id);
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(int id)
        {
            return await Database.UserManager.GetStudentsAsync(id);
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            return await Database.UserManager.GetUserAsync(id);
        }

        public async Task<ApplicationUser> GetUserWithoutTrackingAsync(string id)
        {
            return await Database.UserManager.GetUserWithoutTrackingAsync(id);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUserByRoleAsync(string usersRole)
        {
            return await Database.UserManager.GetUserByRoleAsync(usersRole);
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            return await Database.UserManager.UpdateStudentAsync(student);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await Database.UserManager.UpdateUserAsync(user);
        }
    }
}