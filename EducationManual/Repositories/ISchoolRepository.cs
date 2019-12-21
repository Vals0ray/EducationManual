using System.Collections.Generic;
using System.Threading.Tasks;
using EducationManual.Models;

namespace EducationManual.Repositories
{
    public interface ISchoolRepository
    {
        Task<School> GetSchoolAsync(int id);

        Task<School> GetSchoolByNameAsync(string name);

        Task<School> AddSchoolAsync(School school);

        Task<IEnumerable<School>> GetSchoolsAsync();

        Task DeleteSchoolAsync(int id);

        Task<School> UpdateSchoolAsync(School school);
    }
}