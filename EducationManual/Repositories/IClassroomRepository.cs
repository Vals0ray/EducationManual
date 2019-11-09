using EducationManual.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Repositories
{
    public interface IClassroomRepository
    {
        Task<Classroom> GetClassroomAsync(int id);

        Task<Classroom> AddClassroomAsync(Classroom classroom);

        Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId);

        Task DeleteClassroomAsync(int id);

        Task<Classroom> UpdateClassroomAsync(Classroom classroom);
    }
}