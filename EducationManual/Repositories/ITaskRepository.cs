using EducationManual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EducationManual.Repositories
{
    public interface ITaskRepository
    {
        Task<TaskForStudent> GetTask(int taskId);

        Task<IEnumerable<TaskForStudent>> GetTasks();
    }
}
