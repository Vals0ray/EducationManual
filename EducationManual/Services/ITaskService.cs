using EducationManual.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EducationManual.Services
{
    public interface ITaskService
    {
        Task<TaskForStudent> GetTask(int taskId);

        Task<IEnumerable<TaskForStudent>> GetTasks();
    }
}
