using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zbc_gp_project_frontend.Models
{
    public class TaskModel
    {
        public string Id { get; set; } 
        public string UserId { get; set; } 
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
