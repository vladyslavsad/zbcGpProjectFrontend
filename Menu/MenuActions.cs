using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using zbc_gp_project_frontend.Interfaces;
using zbc_gp_project_frontend.Models;
using zbc_gp_project_frontend.Services;
using static System.Net.WebRequestMethods;

namespace zbc_gp_project_frontend.Menu
{
    public class MenuActions
    {

        private AuthService _authService;
        private TaskService _taskService;

        public MenuActions(HttpClient httpClient) 
        {
             _authService = new AuthService(httpClient);
             _taskService = new TaskService(httpClient);
        }

        

        public async Task<bool> Login()
        {
            Console.WriteLine("=== LOG IND ===");
            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Adgangskode: ");
            var password = Console.ReadLine()?.Trim() ?? "";

            var dto = new { Email = email, Password = password };
            try
            {
                var isLogged = await _authService.Login(email, password);
                if (isLogged)
                {
                    Console.WriteLine("✅ Logget ind!");
                    return true;
                }
                else return false;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Netværksfejl: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Register()
        {
            Console.WriteLine("=== REGISTRÉR ===");
            Console.Write("Email: ");
            var email = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Adgangskode: ");
            var password = Console.ReadLine()?.Trim() ?? "";

            var dto = new { Email = email, Password = password };
            try
            {
                var isSuccessfull = await _authService.Register(email, password);
                if (isSuccessfull)
                {
                    Console.WriteLine("✅ Registreret!");
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                Process.Start("msg", $"* Netværksfejl: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> GetTasks()
        {
            Console.WriteLine("=== VIS OPGAVER ===\n");
            try
            {
                var resp = await _taskService.GetTasks();

                if (resp == null || !resp.Any())
                {
                    Console.WriteLine("Du har ingen opgaver");
                    return false;
                }

                for (int i = 0; i < resp?.Count(); i++)
                {
                    Console.WriteLine($"Opgave n. {i + 1} : {resp[i].Title} \t{resp[i].Description} \t{resp[i].TimeStamp}\n");
                }
                return true;
            }
            catch (Exception ex)
            {
                Process.Start("msg", $"* Netværksfejl: {ex.Message}");
                return false;
            }
        }

         public async Task<bool> AddTask()
        {
            Console.Clear();
            Console.WriteLine("=== Tilføj opgave ===\n");

            Console.Write("Vælg din opgave overskrift : ");
            string title = Console.ReadLine()?.Trim() ?? "";
            Console.Write("Vælg din opgave beskrivelse : ");
            string description = Console.ReadLine()?.Trim() ?? "";


            try
            {

                if (await _taskService.AddTask(title, description))
                {
                    Console.WriteLine("Opgaven er tilføjet");
                    return true;
                }
                else
                {
                    Console.WriteLine("Noget gik galt...");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Process.Start("msg", $"* Netværksfejl: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RemoveTask()
        {
            Console.Clear();
            Console.WriteLine("=== Fjern opgave ===\n");

            try
            {
                var resp = await _taskService.GetTasks();
          
                for (int i = 0; i < resp?.Count(); i++)
                    {
                        Console.WriteLine($"Opgave n. {i + 1} : {resp[i].Title} \t{resp[i].Description} \t{resp[i].TimeStamp}\n");
                }

                if (resp == null || !resp.Any())
                {
                    Console.WriteLine("Du har ingen opgaver");
                    return false;
                }

                Console.Write("Vælg den markeret opgave (intast nummeret) : ");
                string id = Console.ReadLine()?.Trim() ?? "";
                int.TryParse(id, out int selectedId);

                if (await _taskService.DeleteTask(resp[selectedId-1].Id))
                {
                    Console.WriteLine("Task was successfully completed");
                    return true;
                }
                else
                {
                    Console.WriteLine("Something went wrong...");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Process.Start("msg", $"* Netværksfejl: {ex.Message}");
                return false;
            }
        }
    }
}
