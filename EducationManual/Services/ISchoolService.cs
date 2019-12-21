using EducationManual.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public interface ISchoolService
    {
        Task<School> GetSchoolAsync(int id);

        Task<School> GetSchoolByNameAsync(string name);

        Task<School> AddSchoolAsync(School school);

        Task<IEnumerable<School>> GetSchoolsAsync();

        Task DeleteSchoolAsync(int id);

        Task<School> UpdateSchoolAsync(School school);
    }
}