using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using EducationManual.Models;
using EducationManual.Repositories;

namespace EducationManual.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskForStudent> GetTask(int taskId)
        {
            return await _taskRepository.GetTask(taskId);
        }

        public async Task<IEnumerable<TaskForStudent>> GetTasks()
        {
            return await _taskRepository.GetTasks();
        }
    }
}