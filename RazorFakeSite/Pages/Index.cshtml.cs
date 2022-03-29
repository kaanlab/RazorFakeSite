using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RazorFakeSite.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RazorFakeSite.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        [Required]
        public string Username { get; set; }

        [BindProperty]
        [Required]
        public string Password { get; set; }

        public string Msg { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            LoginModel loginModel = ReadFromFile();
            if (ModelState.IsValid)
            {
                if (Username.Equals(loginModel.Username) && Password.Equals(loginModel.Password))
                {
                    HttpContext.Session.SetString("username", Username);
                    return RedirectToPage("Admin");
                }

                Msg = "Wrong username or password!";
                return Page();
            }

            Msg = "Invalid!";
            return Page();

        }

        private LoginModel ReadFromFile()
        {
            string fileName = "login.txt";
            if (System.IO.File.Exists(fileName))
            {
                string jsonString = System.IO.File.ReadAllText(fileName);
                return JsonSerializer.Deserialize<LoginModel>(jsonString);
            }
            return new LoginModel { Username = "abc", Password = "123" };
        }
    }
}
