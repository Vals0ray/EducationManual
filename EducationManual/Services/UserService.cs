using EducationManual.Models;
using EducationManual.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace EducationManual.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            return await _userRepository.AddStudentAsync(student);
        }

        public async Task DeleteStudentAsync(int id)
        {
            await _userRepository.DeleteStudentAsync(id);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<Student> GetStudentAsync(int id)
        {
            return await _userRepository.GetStudentAsync(id);
        }

        public async Task<IEnumerable<Student>> GetStudentsAsync(int id)
        {
            return await _userRepository.GetStudentsAsync(id);
        }

        public async Task<ApplicationUser> GetUserAsync(string id)
        {
            return await _userRepository.GetUserAsync(id);
        }

        public async Task<Student> UpdateStudentAsync(Student student)
        {
            return await _userRepository.UpdateStudentAsync(student);
        }

        public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
        {
            return await _userRepository.UpdateUserAsync(user);
        }
    }
}