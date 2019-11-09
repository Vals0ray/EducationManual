using EducationManual.Repositories;
using System.Collections.Generic;
using EducationManual.Models;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public class ClassroomService : IClassroomService
    {
        private readonly IClassroomRepository _classroomRepository;

        public ClassroomService(IClassroomRepository classroomRepository)
        {
            _classroomRepository = classroomRepository;
        }

        public async Task<Classroom> AddClassroomAsync(Classroom classroom)
        {
            return await _classroomRepository.AddClassroomAsync(classroom);
        }

        public async Task DeleteClassroomAsync(int id)
        {
            await _classroomRepository.DeleteClassroomAsync(id);
        }

        public async Task<Classroom> GetClassroomAsync(int id)
        {
            return await _classroomRepository.GetClassroomAsync(id);
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId)
        {
            return await _classroomRepository.GetClassroomsAsync(schoolId);
        }

        public async Task<Classroom> UpdateClassroomAsync(Classroom classroom)
        {
            return await _classroomRepository.UpdateClassroomAsync(classroom);
        }
    }
}