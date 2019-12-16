using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using EducationManual.Models;

namespace EducationManual.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public async Task<TaskForStudent> GetTask(int taskId)
        {
            TaskForStudent task = null;

            using (var db = new ApplicationContext())
            {
                task = await db.TaskForStudents.FirstOrDefaultAsync(t => t.TaskId == taskId);
            }

            return task;
        }

        public async Task<IEnumerable<TaskForStudent>> GetTasks()
        {
            var tasks = new List<TaskForStudent>();

            using (var db = new ApplicationContext())
            {
                tasks = await db.TaskForStudents.ToListAsync();
            }

            return tasks;
        }
    }
}