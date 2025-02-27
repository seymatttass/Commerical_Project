﻿using Users.API.DTOS.Users;
using Users.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Users.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Tüm kullanıcıları getirir.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kullanıcıyı getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound($"{id} ID'li kullanıcı bulunamadı.");
            return Ok(user);
        }

        /// <summary>
        /// Yeni kullanıcı oluşturur.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDTO createUserDto)
        {
            var result = await _userService.AddAsync(createUserDto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        /// <summary>
        /// Kullanıcı bilgilerini günceller.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDTO updateUserDto)
        {
            if (id != updateUserDto.Id)
                return BadRequest("URL'deki ID ile DTO'daki ID eşleşmiyor.");

            var result = await _userService.UpdateAsync(updateUserDto);
            if (!result)
                return NotFound($"{id} ID'li kullanıcı bulunamadı.");

            return NoContent();
        }

        /// <summary>
        /// Kullanıcıyı siler.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteAsync(id);
            if (!result)
                return NotFound($"{id} ID'li kullanıcı bulunamadı.");

            return NoContent();
        }
    }
}
