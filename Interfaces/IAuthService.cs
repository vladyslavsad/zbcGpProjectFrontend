using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zbc_gp_project_frontend.Interfaces
{
    internal interface IAuthService
    {
        public Task<bool> Login(string email, string password);
        public Task<bool> Register(string email, string password);
    }
}
