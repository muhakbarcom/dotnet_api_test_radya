using Dtos.User;
using Extensions;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminOrUser")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly iTokenService _tokensService;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, iTokenService tokensService, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _tokensService = tokensService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Getting all users");

            try
            {
                var users = _userManager.Users.ToList();

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all users");
                return StatusCode(500, "Internal server error. See log for details.");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetById(string id)
        {
            _logger.LogInformation("Getting user with id {Id}", id);

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with id {Id} not found", id);
                    return NotFound();
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting user with id {Id}", id);
                return StatusCode(500, "Internal server error. See log for details.");
            }
        }


        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateUserDto req)
        {
            _logger.LogInformation("Creating user with username {Username}", req.Username);

            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var user = new User
                {
                    UserName = req.Username,
                    Email = req.Email,
                    FirstName = req.FirstName,
                    LastName = req.LastName
                };

                if (!await _roleManager.RoleExistsAsync(req.Role)) return BadRequest("Role does not exist.");
                var result = await _userManager.CreateAsync(user, req.Password);

                if (!result.Succeeded)
                {
                    _logger.LogError("User creation failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(result.Errors);
                }

                var roleAssignResult = await _userManager.AddToRoleAsync(user, req.Role);
                if (!roleAssignResult.Succeeded)
                {
                    _logger.LogError("Adding role failed: {Errors}", string.Join(", ", roleAssignResult.Errors.Select(e => e.Description)));
                    return BadRequest(roleAssignResult.Errors);
                }

                return Ok(new NewUserDto
                {
                    Username = user.UserName,
                    Email = user.Email
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                return StatusCode(500, "Internal server error. See log for details.");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateUserDto req)
        {
            _logger.LogInformation("Updating user with id {Id}", id);

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with id {Id} not found", id);
                    return NotFound();
                }

                user.FirstName = req.FirstName;
                user.LastName = req.LastName;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("User update failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(result.Errors);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating user with id {Id}", id);
                return StatusCode(500, "Internal server error. See log for details.");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting user with id {Id}", id);

            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    _logger.LogWarning("User with id {Id} not found", id);
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);

                if (!result.Succeeded)
                {
                    _logger.LogError("User deletion failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return BadRequest(result.Errors);
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting user with id {Id}", id);
                return StatusCode(500, "Internal server error. See log for details.");
            }
        }

        // update profile
        [HttpPut("profile")]
        [Authorize(Policy = "AdminOrUser")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto req)
        {
            _logger.LogInformation("Updating profile for user {Username}", User.Identity.Name);

            try
            {
                var username = User.GetUsername();
                var user = await _userManager.FindByNameAsync(username);

                if (user == null)
                {
                    _logger.LogWarning($"User with username {username} not found", User.Identity.Name);
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(req.CurrentPassword) && !string.IsNullOrEmpty(req.NewPassword))
                {
                    var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);

                    if (!result.Succeeded)
                    {
                        _logger.LogError("Password change failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                        return BadRequest(result.Errors);
                    }
                }

                if (!string.IsNullOrEmpty(req.FirstName))
                    user.FirstName = req.FirstName;

                if (!string.IsNullOrEmpty(req.LastName))
                    user.LastName = req.LastName;

                var resultUpdate = await _userManager.UpdateAsync(user);

                if (!resultUpdate.Succeeded)
                {
                    _logger.LogError("User update failed: {Errors}", string.Join(", ", resultUpdate.Errors.Select(e => e.Description)));
                    return BadRequest(resultUpdate.Errors);
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating profile for user {Username}", User.Identity.Name);
                return StatusCode(500, "Internal server error. See log for details.");
            }
        }
    }
}
