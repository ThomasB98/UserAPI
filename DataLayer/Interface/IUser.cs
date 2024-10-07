using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model.Dto;
using DataLayer.Constants.ResponceEntity;

namespace DataLayer.Interface
{
    public interface IUser
    {
        Task<ResponseBody<UserDto>> GetUserByIdASync(int id);
        Task<ResponseBody<UserDto>> CreateUserAsync(UserDto user);
        Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto user);
        Task<ResponseBody<bool>> DeleteUserAsync(int userId);
        Task<ResponseBody<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<ResponseBody<string>> Login(string Email, string password);
        Task<ResponseBody<bool>> ForgetPassword(string Email,string newPassword);
    }
}
