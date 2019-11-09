using EducationManual.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public interface IClassroomService
    {
        Task<Classroom> GetClassroomAsync(int id);

        Task<Classroom> AddClassroomAsync(Classroom classroom);

        Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId);

        Task DeleteClassroomAsync(int id);

        Task<Classroom> UpdateClassroomAsync(Classroom classroom);
    }
}