using Users.API.DTOS.Users;
using Users.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Fetching all users.");
            var users = await _userService.GetAllAsync();
            if (users == null || !users.Any())
            {
                _logger.LogWarning("No users found.");
            }
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation($"Fetching user with ID: {id}");
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning($"User with ID: {id} not found.");
                return NotFound($"{id} ID'li kullanıcı bulunamadı.");
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDto)
        {
            _logger.LogInformation("Creating a new user.");
            var result = await _userService.AddAsync(createUserDto);
            _logger.LogInformation($"User created with ID: {result.Id}");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO updateUserDto)
        {
            _logger.LogInformation($"Updating user with ID: {id}");
            if (id != updateUserDto.Id)
            {
                _logger.LogWarning($"ID mismatch: URL ID = {id}, DTO ID = {updateUserDto.Id}");
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor.");
            }

            var result = await _userService.UpdateAsync(updateUserDto);
            if (!result)
            {
                _logger.LogWarning($"User with ID: {id} not found for update.");
                return NotFound($"{id} ID'li kullanıcı bulunamadı.");
            }

            _logger.LogInformation($"User with ID: {id} updated successfully.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting user with ID: {id}");
            var result = await _userService.DeleteAsync(id);
            if (!result)
            {
                _logger.LogWarning($"User with ID: {id} not found for deletion.");
                return NotFound($"{id} ID'li kullanıcı bulunamadı.");
            }

            _logger.LogInformation($"User with ID: {id} deleted successfully.");
            return NoContent();
        }
    }
}
