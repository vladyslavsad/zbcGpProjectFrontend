using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using zbc_gp_project_frontend.Interfaces;
using zbc_gp_project_frontend.Models;

namespace zbc_gp_project_frontend.Services
{
    public class TaskService : ITaskService
    {
        static readonly string GetLabours = "/api/Labour/getLabours";
        static readonly string AddLabour = "/api/Labour/addLabour";
        static readonly string DeleteLabour = "/api/Labour/deleteLabour";

        private HttpClient _httpClient;
        public TaskService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<bool> AddTask(string title, string description)
        {
            try
            {
                var taskDto = new { Title = title, Description = description };
                var resp = await _httpClient.PostAsJsonAsync(AddLabour, taskDto);
                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Nerværksfejl " + ex.Message);
                return false;
            }
           
        }

        public async Task<bool> DeleteTask(string id)
        {
            try
            {
                var resp = await _httpClient.DeleteAsync($"{DeleteLabour}?id={id}");
                if (resp.IsSuccessStatusCode)
                {
                    return true;
                }
                else return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Nerværksfejl " + ex.Message);
                return false;
            }
        }

         public async Task<List<TaskModel>> GetTasks()
        {
            List<TaskModel> jsonResp;
            try
            {
                var resp = await _httpClient.GetAsync(GetLabours);
                if (resp.IsSuccessStatusCode)
                {
                    jsonResp = await resp?.Content?.ReadFromJsonAsync<List<TaskModel>>();
                    if (jsonResp.Any())
                    {
                        return jsonResp.ToList();
                    }
                    else return null;
                }
                else return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Nerværksfejl " + ex.Message);
                return null;
            }
        }
    }
}
