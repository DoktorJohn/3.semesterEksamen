using APIAdvokaten.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using APIAdvokaten.ViewModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using APIAdvokaten.Data;
using Microsoft.Extensions.Configuration;

namespace APIAdvokaten.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> signInManager;
        private readonly UserManager<AppUser> userManager;
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, AppDbContext dbContext, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Normalize the username to match the database
            var normalizedUserName = model.Username.ToUpperInvariant();

            // Now find the user by normalized username
            var user = await userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == normalizedUserName);

            if (user == null)
            {
                return Unauthorized("User not found");
            }

            var result = await signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            if (result.IsLockedOut)
            {
                return Unauthorized("User is locked out");
            }

            if (result.IsNotAllowed)
            {
                return Unauthorized("User is not allowed to sign in");
            }

            return Unauthorized("Invalid login attempt");
        }

        private string GenerateJwtToken(AppUser user)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, user.UserName), // Use UserName instead of Email if that's intended
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            // Use the configured key from appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"], // Pull from appsettings
                audience: _configuration["Jwt:Audience"], // Pull from appsettings
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                UserName = model.Email,  // Assuming you use email as username
                Email = model.Email,
                Name = model.Name,       // Ensure this is populated
                Address = model.Address
            };

            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok("User registered successfully");
            }

            // Return errors if registration fails
            return BadRequest(result.Errors);
        }
    }
}