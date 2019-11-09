using EducationManual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EducationManual.Services
{
    public interface IUserService
    {
        Task DeleteUserAsync(string id);

        Task<ApplicationUser> GetUserAsync(string id);

        Task<ApplicationUser> UpdateUserAsync(ApplicationUser user);

        Task<IEnumerable<Student>> GetStudentsAsync(int id);

        Task<Student> AddStudentAsync(Student student);

        Task DeleteStudentAsync(int id);

        Task<Student> GetStudentAsync(int id);

        Task<Student> UpdateStudentAsync(Student student);
    }
}