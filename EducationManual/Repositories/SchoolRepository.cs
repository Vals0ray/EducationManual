using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using EducationManual.Models;

namespace EducationManual.Repositories
{
    public class SchoolRepository : ISchoolRepository
    {
        public async Task<School> GetSchoolAsync(int id)
        {
            School result = null;

            using (var db = new ApplicationContext())
            {
                result = await db.Schools.Include(s => s.ApplicationUsers)
                                         .Include(s => s.Classrooms.Select(c => c.Students))
                                         .FirstOrDefaultAsync(s => s.SchoolId == id);
            }

            return result;
        }

        public async Task<School> GetSchoolByNameAsync(string name)
        {
            School result = null;

            using (var db = new ApplicationContext())
            {
                result = await db.Schools.Include(s => s.ApplicationUsers)
                                         .Include(s => s.Classrooms.Select(c => c.Students))
                                         .FirstOrDefaultAsync(s => s.Name == name);
            }

            return result;
        }

        public async Task<School> AddSchoolAsync(School school)
        {
            School result = null;

            using (var db = new ApplicationContext())
            {
                result = db.Schools.Add(school);
                await db.SaveChangesAsync();
            }

            return result;
        }

        public async Task<IEnumerable<School>> GetSchoolsAsync()
        {
            var result = new List<School>();

            using (var db = new ApplicationContext())
            {
                result = await db.Schools.Include(s => s.ApplicationUsers)
                                         .Include(s => s.Classrooms)
                                         .ToListAsync();
            }

            return result;
        }

        // TODO: Delete related data
        public async Task DeleteSchoolAsync(int id)
        {
            using (var db = new ApplicationContext())
            {
                var school = await db.Schools.Include(u => u.ApplicationUsers)
                                             .Include(s => s.Classrooms.Select(c => c.Students))
                                             .FirstOrDefaultAsync(s => s.SchoolId == id);

                var classroomsId = school.Classrooms.Select(c => c.ClassroomId).ToList();
                var usersId = school.ApplicationUsers.Select(u => u.Id).ToList();

                db.Entry(school).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                // Delete all classrooms with students in the School
                if (classroomsId.Count != 0)
                {
                    foreach (var classroom in classroomsId)
                    {
                        ClassroomRepository classroomRepository = new ClassroomRepository();
                        await classroomRepository.DeleteClassroomAsync(classroom);
                    }
                }

                // Delete all users in the School
                if (usersId.Count != 0)
                {
                    foreach (var user in usersId)
                    {
                        if (db.Users.Any(u => u.Id == user))
                        {
                            UserRepository userRepository = new UserRepository();
                            await userRepository.DeleteUserAsync(user);
                        }
                    }
                }
            }
        }

        public async Task<School> UpdateSchoolAsync(School school)
        {
            using (var db = new ApplicationContext())
            {
                db.Entry(school).State = EntityState.Modified;

                await db.SaveChangesAsync();
            }

            return school;
        }
    }
}