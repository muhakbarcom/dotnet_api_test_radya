
using Dtos.Auth;
using Dtos.User;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly iTokenService _tokensService;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserController> _logger;
        public AuthController(UserManager<User> userManager, iTokenService tokensService, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _tokensService = tokensService;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto req)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == req.Username.ToLower());

                if (user == null) return Unauthorized("Invalid username or password");

                var result = await _signInManager.CheckPasswordSignInAsync(user, req.Password, false);

                if (!result.Succeeded) return Unauthorized("Invalid username or password");

                return Ok(
                    new NewUserDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokensService.CreateToken(user)
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto req)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var user = new User
                {
                    UserName = req.Username,
                    Email = req.Email,
                    FirstName = req.FirstName,
                    LastName = req.LastName
                };

                string role = "CUSTOMER";

                if (!await _roleManager.RoleExistsAsync(role)) return BadRequest($"Role {role} does not exist.");

                var addUser = await _userManager.CreateAsync(user, req.Password);

                if (!addUser.Succeeded) return BadRequest(addUser.Errors);

                var roleAssignResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleAssignResult.Succeeded)
                {
                    _logger.LogError("Adding role failed: {Errors}", string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)));
                    return BadRequest(roleAssignResult.Errors);
                }

                return Ok(
                    new NewUserDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokensService.CreateToken(user)
                    }
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok("Logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}