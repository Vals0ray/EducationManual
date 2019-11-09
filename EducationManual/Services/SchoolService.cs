using EducationManual.Models;
using EducationManual.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public class SchoolService : ISchoolService
    {
        private readonly ISchoolRepository _schoolRepository;

        public SchoolService(ISchoolRepository schoolRepository)
        {
            _schoolRepository = schoolRepository;
        }

        public async Task<School> AddSchoolAsync(School school)
        {
            return await _schoolRepository.AddSchoolAsync(school);
        }

        public async Task DeleteSchoolAsync(int id)
        {
            await _schoolRepository.DeleteSchoolAsync(id);
        }

        public async Task<School> GetSchoolAsync(int id)
        {
            return await _schoolRepository.GetSchoolAsync(id);
        }

        public async Task<IEnumerable<School>> GetSchoolsAsync()
        {
            return await _schoolRepository.GetSchoolsAsync();
        }

        public async Task<School> UpdateSchoolAsync(School school)
        {
            return await _schoolRepository.UpdateSchoolAsync(school);
        }
    }
}