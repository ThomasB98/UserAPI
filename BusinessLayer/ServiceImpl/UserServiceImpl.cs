using BusinessLayer.Service;
using DataLayer.Constants.ResponceEntity;
using ModelLayer.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Interface;

namespace BusinessLayer.ServiceImpl
{
    public class UserServiceImpl : IUserService
    {

        private readonly IUser _userRespo;

        public UserServiceImpl(IUser userRepo)
        {
            _userRespo = userRepo;
        }

        public Task<ResponseBody<UserDto>> CreateUserAsync(UserDto userDto)
        {
            return _userRespo.CreateUserAsync(userDto);
        }

        public Task<ResponseBody<bool>> DeleteUserAsync(int userId)
        {
            return _userRespo.DeleteUserAsync(userId);
        }

        public Task<ResponseBody<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            return _userRespo.GetAllUsersAsync();
        }

        public Task<ResponseBody<UserDto>> GetUserByIdAsync(int userId)
        {
            return _userRespo.GetUserByIdASync(userId);    
        }

        public Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto updatedUserDto)
        {
            return _userRespo.UpdateUserAsync(updatedUserDto);
        }


        public Task<ResponseBody<string>> LoginAsync(string email,string password)
        {
            return _userRespo.Login(email, password);
        }


        public Task<ResponseBody<bool>> ForgetPassword(string email,string newPassword)
        {
            return _userRespo.ForgetPassword(email,newPassword);
        }

    }
}
