using EducationManual.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetUserAsync(string id);

        //Task<ApplicationUser> GetUserByRoleAsync(int id);

        Task DeleteUserAsync(string id);

        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);

        // 

        Task<IEnumerable<Student>> GetStudentsAsync(int id);

        Task<Student> GetStudentAsync(string id);

        Task<Student> AddStudentAsync(Student student);

        Task DeleteStudentAsync(string id);

        Task<Student> UpdateStudentAsync(Student student);

        //
    }
}