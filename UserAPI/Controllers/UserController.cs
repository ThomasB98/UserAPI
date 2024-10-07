using BusinessLayer.Service;
using DataLayer.Constants.ResponceEntity;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<ActionResult<ResponseBody<IEnumerable<UserDto>>>> GetAllUsersAsync()
        {
           return await _userService.GetAllUsersAsync();
        }

        [HttpPost(Name = "register")]
        [AllowAnonymous]
        public async Task<ResponseBody<UserDto>> CreateUserAsync(UserDto userDto)
        {
            return await _userService.CreateUserAsync(userDto);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<ResponseBody<bool>>> DeleteUserAsync(int id)
        {
            return await _userService.DeleteUserAsync(id);
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize]
        public async Task<ActionResult<ResponseBody<UserDto>>> GetUserByIdAsync(int id)
        {
            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseBody<UserDto>>> UpdateUserAsync([FromBody]UserDto updatedUserDto)
        {
            return await _userService.UpdateUserAsync(updatedUserDto);
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseBody<string>>> Login([FromBody]LoginModel loginModel)
        {
          return await _userService.LoginAsync(loginModel.email, loginModel.password);
        }


        [HttpPut("ForgetPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseBody<bool>>> forgetPassword(string email, string newPassword)
        {
            return await _userService.ForgetPassword(email,newPassword);
        }
    }
}
