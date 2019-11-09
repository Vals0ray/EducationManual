using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EducationManual.Models;

namespace EducationManual.Repositories
{
    public class ClassroomRepository : IClassroomRepository
    {
        public async Task<Classroom> AddClassroomAsync(Classroom classroom)
        {
            Classroom result = null;

            using (var db = new ApplicationContext())
            {
                result = db.Classrooms.Add(classroom);
                await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task DeleteClassroomAsync(int id)
        {
            using (var db = new ApplicationContext())
            {
                var classroom = await db.Classrooms.FirstOrDefaultAsync(c => c.ClassroomId == id);

                db.Entry(classroom).State = EntityState.Deleted;

                await db.SaveChangesAsync();
            }
        }

        public async Task<Classroom> GetClassroomAsync(int id)
        {
            Classroom result = null;

            using (var db = new ApplicationContext())
            {
                result = await db.Classrooms.Include(c => c.School)
                                            .Include(c => c.Students)
                                            .FirstOrDefaultAsync(c => c.ClassroomId == id);
            }

            return result;
        }

        public async Task<IEnumerable<Classroom>> GetClassroomsAsync(int schoolId)
        {
            var result = new List<Classroom>();

            using (var db = new ApplicationContext())
            {
                result = await db.Classrooms.Include(c => c.School)
                                            .Include(c => c.Students)
                                            .Where(c => c.SchoolId == schoolId)
                                            .ToListAsync();
            }

            return result;
        }

        public async Task<Classroom> UpdateClassroomAsync(Classroom classroom)
        {
            using (var db = new ApplicationContext())
            {
                db.Entry(classroom).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }

            return classroom;
        }
    }
}