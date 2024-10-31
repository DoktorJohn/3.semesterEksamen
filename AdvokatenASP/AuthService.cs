using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AdvokatenASP.Models;

namespace AdvokatenASP
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var loginData = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync("/api/account/login", loginData); // Ensure the correct endpoint

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>(); // Assuming the token is returned in a JSON object
                var token = result.Token; // Extract the token
                                          // Store token in session
                _httpContextAccessor.HttpContext.Session.SetString("Token", token);
                return token;
            }

            throw new Exception("Login failed: " + response.ReasonPhrase);
        }

        public async Task<UserResponse> GetProtectedResourceAsync()
        {
            var token = _httpContextAccessor.HttpContext.Session.GetString("Token");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                throw new Exception("No token found in session.");
            }

            var response = await _httpClient.GetAsync("api/protected-resource");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            throw new Exception("Failed to retrieve protected resource: " + response.ReasonPhrase);
        }
    }
}
