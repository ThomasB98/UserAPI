
using DataLayer.Constants.ResponceEntity;
using ModelLayer.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BusinessLayer.Service
{
    public interface IUserService
    {
        Task<ResponseBody<UserDto>> CreateUserAsync(UserDto userDto);
        Task<ResponseBody<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ResponseBody<UserDto>> GetUserByIdAsync(int userId);
        Task<ResponseBody<bool>> DeleteUserAsync(int userId);
        Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto updatedUserDto);

    }
}
