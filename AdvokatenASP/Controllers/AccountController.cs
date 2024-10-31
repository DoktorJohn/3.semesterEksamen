using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AdvokatenASP.Controllers;
using AdvokatenASP.Models;
using AdvokatenASP.ViewModels;
using System.Text;

namespace AdvokatenASP.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpGet("register")]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }



        public async Task<IActionResult> ApiLogin(LoginVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model); 
            }

            using var client = new HttpClient();
            var requestUrl = "https://localhost:7195/api/account/login";
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                Username = model.Username,
                Password = model.Password
            }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                // Assuming a successful response contains the token as JSON
                var responseContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<TokenResponse>(responseContent)?.Token;

                // Store the token, e.g., in session or a cookie
                HttpContext.Session.SetString("JwtToken", token!);
                return RedirectToAction("Index", "Home"); // Redirect on successful login
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", errorResponse);
                return View(model); // Return view with error message
            }
        }

        public async Task<IActionResult> ApiRegister(RegisterVM rvm)
        {
            if (!ModelState.IsValid)
            {
                return View(rvm);
            }

            var client = new HttpClient();
            var requestUrl = "https://localhost:7195/api/account/register";
            var content = new StringContent(JsonConvert.SerializeObject(rvm), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", errorResponse);
                return View(rvm);
            }

        }
    }
}