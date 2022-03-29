using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorFakeSite.Model;
using System.Text.Json;

namespace RazorFakeSite.Pages
{
    public class AdminModel : PageModel
    {
        public string Username { get; set; }

        public string Result { get; set; }

        public IActionResult OnGet()
        {
            Username = HttpContext.Session.GetString("username");
            Result = ReadPasswordFromFile();
            return Page();
        }

        public IActionResult OnGetLogout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToPage("Index");
        }


        [BindProperty]
        public LoginModel LoginModel { get; set; }
        public IActionResult OnPost()
        {
            string json = JsonSerializer.Serialize(LoginModel);
            System.IO.File.WriteAllText("login.txt", json);
            Result = ReadPasswordFromFile();
            return Page();
        }

        private string ReadPasswordFromFile()
        {
            string fileName = "login.txt";
            if (System.IO.File.Exists(fileName))
            {
                string jsonString = System.IO.File.ReadAllText(fileName);
                var loginReault = JsonSerializer.Deserialize<LoginModel>(jsonString);
                return $"{loginReault.Username} :: {loginReault.Password}";
            }

            return "file doesn't exist!";
        }
    }
}
