using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using zbc_gp_project_frontend.Models;

namespace zbc_gp_project_frontend.Interfaces
{
    internal interface ITaskService
    {
        public Task<List<TaskModel>> GetTasks();
        public Task<bool> DeleteTask(string id);
        public Task<bool> AddTask(string title, string description);
    }
}
