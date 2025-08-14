using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using zbc_gp_project_frontend.Interfaces;
using zbc_gp_project_frontend.Models;
using static Program;

namespace zbc_gp_project_frontend.Services
{
    public class AuthService : IAuthService
    {
        static readonly string LoginPath = "api/Auth/login";
        static readonly string RegisterPath = "api/Auth/register";

        private HttpClient _httpClient;
        public AuthService(HttpClient httpClient) 
        {
            _httpClient = httpClient;
        }

        public async Task<bool> Login( string email, string password)
        {
            var dto = new { Email = email, Password = password };
            try
            {
                var resp = await _httpClient.PostAsJsonAsync(LoginPath, dto);
                if (!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine("Forkert brugernavn eller adgangskode");
                    return false;
                }
                var json = await resp.Content.ReadFromJsonAsync<TokenResponseModel>();
                var accessToken = json?.access_token;
      
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", accessToken);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Netværksfejl: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> Register(string email, string password)
        {

            var dto = new { Email = email, Password = password };
            try
            {
                var resp = await _httpClient.PostAsJsonAsync(RegisterPath, dto);
                if (!resp.IsSuccessStatusCode)
                {
                    Console.WriteLine("Registration er fejlet");
                    return false;
                }
               
                await Login(email, password);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Netværksfejl: " + ex.Message);
                return false;
            }
        }
    }
}
