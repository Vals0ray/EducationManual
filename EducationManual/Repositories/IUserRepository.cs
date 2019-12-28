using EducationManual.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetUserAsync(string id);

        Task<ApplicationUser> GetUserWithoutTrackingAsync(string id);

        Task<IEnumerable<ApplicationUser>> GetUserByRoleAsync(string usersRole);

        Task DeleteUserAsync(string id);

        Task<IdentityResult> UpdateUserAsync(ApplicationUser user);

        // 

        Task<IEnumerable<Student>> GetStudentsAsync(int id);

        Task<Student> GetStudentAsync(string id);

        Task<Student> AddStudentAsync(Student student);

        Task DeleteStudentAsync(string id);

        Task<Student> UpdateStudentAsync(Student student);

        //
    }
}