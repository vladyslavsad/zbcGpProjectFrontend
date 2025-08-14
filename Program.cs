using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using zbc_gp_project_frontend.Menu;
using zbc_gp_project_frontend.Models;

class Program
{
    static string[] menuItems = {
        "Tilføj opgave",
        "Vis opgaver",
        "Marker opgave som færdig",
        "Afslut"
    };

    static string[] authMenuItems = {
        "Log ind",
        "Registrér",
        "Afslut"
    };

    static int selectedIndex = 0;
    static int authSelectedIndex = 0;
    static string? accessToken;
    static bool loggedIn = false;

    static readonly string BaseUrl = "https://localhost:7136/";
    
    

    static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;

        using var httpContext = new HttpClient { BaseAddress = new Uri(BaseUrl) };
        MenuActions menuActions = new MenuActions(httpContext);

        while (!loggedIn)
        {
            DrawAuthMenu();

            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.UpArrow)
                authSelectedIndex = (authSelectedIndex - 1 + authMenuItems.Length) % authMenuItems.Length;
            else if (key == ConsoleKey.DownArrow)
                authSelectedIndex = (authSelectedIndex + 1) % authMenuItems.Length;
            else if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                if (authSelectedIndex == 0)
                {
                    loggedIn = await menuActions.Login();
                    if (!loggedIn) Console.WriteLine("Forkert adgangskode");
                }
                else if (authSelectedIndex == 1)
                {
                    loggedIn = await menuActions.Register();
                    if (!loggedIn) Console.WriteLine("Uvalid data");
                }
                else if (authSelectedIndex == 2)
                {
                    return;
                }
            }
        }

        while (loggedIn)
        {
            DrawMenu();

            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.UpArrow)
                selectedIndex = (selectedIndex - 1 + menuItems.Length) % menuItems.Length;
            else if (key == ConsoleKey.DownArrow)
                selectedIndex = (selectedIndex + 1) % menuItems.Length;
            else if (key == ConsoleKey.Enter)
            {
                Console.Clear();
                if (selectedIndex == menuItems.Length - 1)
                    break;
                else if (selectedIndex == menuItems.Length - 2)
                {
                    await menuActions.RemoveTask();
                }
                else if (selectedIndex == menuItems.Length - 3)
                {
                    await menuActions.GetTasks();
                }
                else if (selectedIndex == menuItems.Length - 4)
                {
                    await menuActions.AddTask();
                }
                Console.WriteLine("Tryk på en vilkårlig tast for at fortsætte...");
                Console.ReadKey(true);
            }
        }
    }

    static void DrawAuthMenu()
    {
        Console.Clear();
        Console.WriteLine("=== LOGIN SIDE ===\n");

        for(int i = 0; i < authMenuItems.Length; i++ )
        {
            if(i == authSelectedIndex)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else
            {
                Console.ResetColor();
            }
            Console.WriteLine($"{i+1}. {authMenuItems[i]}");
        }
        Console.ResetColor();
    }

    static void DrawMenu()
    {
        Console.Clear();
        Console.WriteLine("=== OPGAVE APP ===\n");
        Console.WriteLine($"Status: {(!loggedIn ? "Ikke logget ind" : "Logget ind ✅")}\n");

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ResetColor();
            }
            Console.WriteLine($"{i + 1}. {menuItems[i]}");
        }
        Console.ResetColor();
    }
}
