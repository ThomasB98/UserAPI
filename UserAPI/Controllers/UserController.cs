using BusinessLayer.Service;
using DataLayer.Constants.ResponceEntity;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model.Dto;

namespace UserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<ResponseBody<IEnumerable<UserDto>>>> GetAllUsersAsync()
        {
           return await _userService.GetAllUsersAsync();
        }

        [HttpPost(Name = "register")]
        public async Task<ResponseBody<UserDto>> CreateUserAsync(UserDto userDto)
        {
            return await _userService.CreateUserAsync(userDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseBody<bool>>> DeleteUserAsync(int id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<ActionResult<ResponseBody<UserDto>>> GetUserByIdAsync(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseBody<UserDto>>> UpdateUserAsync([FromBody]UserDto updatedUserDto)
        {
            return await _userService.UpdateUserAsync(updatedUserDto);
        }

    }
}
