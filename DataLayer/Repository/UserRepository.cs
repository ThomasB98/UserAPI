using DataLayer.Constants.dbConnection;
using DataLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Constants.Exceptions;
using DataLayer.Constants.ResponceEntity;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.Dto;
using ModelLayer.Model.Entity;
using AutoMapper;
using System.Net;
using BCrypt.Net;
using DataLayer.Constants.JwtTokenGen;

namespace DataLayer.Repository
{
    public class UserRepository : IUser
    {   
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IAddress _address;
        private readonly LoginRepository _loginRepository;
        private readonly TokenGenerater _tokenGenerater;

        public UserRepository(DataContext dataContext,IMapper mapper, IAddress address,LoginRepository loginRepository,TokenGenerater tokenGenerater)     
        {
            _context = dataContext;
            _mapper = mapper;
            _address = address;
            _loginRepository = loginRepository;
            _tokenGenerater = tokenGenerater;
        }

        public async Task<ResponseBody<string>> Login(string Email, string password)
        {
            var responce = new ResponseBody<string>();
            try
            {
                var user = await _loginRepository.Login(Email, password);
                if (user!=null)
                {
                    var token = _tokenGenerater.GenerateToken(user);
                    responce.Data = token;
                    responce.Success = true;
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Message = "Login Successfull";
                    return responce;
                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                responce.Data = "";
                responce.Success = false;
                responce.StatusCode = HttpStatusCode.NotFound;
                responce.Message = "Login unSuccessfull";
                return responce;
            }
        }

        public async Task<ResponseBody<UserDto>> CreateUserAsync(UserDto userDto)
        {
            // Await the asynchronous database query
            var entity = await _context.User.FirstOrDefaultAsync(user => user.Email.Equals(userDto.Email));
            var response = new ResponseBody<UserDto>();

            // Check if the user already exists
            if (entity == null)
            {
                try
                {
                    var user = _mapper.Map<User>(userDto);
                    user.CreatedDate = DateTime.Now;
                    user.Password = HashPassword(userDto.Password);

                    // Add user to the context and save changes
                    _context.User.Add(user);
                    int changes = await _context.SaveChangesAsync();

                    if (changes > 0)
                    {
                        // Map and add the address after user has been created
                        var address = _mapper.Map<Address>(userDto.Address);
                        address.UserId = user.Id;
                        _context.Address.Add(address);
                        await _context.SaveChangesAsync();

                        // Respond with success
                        response.Data = userDto;
                        response.Success = true;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = "User registered successfully";
                    }
                    else
                    {
                        response.Success = false;
                        response.StatusCode = HttpStatusCode.InternalServerError;
                        response.Message = "Failed to save user";
                    }
                }
                catch (Exception ex)
                {
                    // Handle exceptions and log the error if necessary
                    response.Success = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"An error occurred: {ex.Message}";
                }
            }
            else
            {
                // Respond if user already exists
                response.Success = false;
                response.StatusCode = HttpStatusCode.Conflict; // 409 Conflict
                response.Message = "User with this email already exists";
            }

            return response;
        }

        public async Task<ResponseBody<bool>> DeleteUserAsync(int userId)
        {
            var responce = new ResponseBody<bool>();
            var character = await _context.User.FirstOrDefaultAsync(x => x.Id == userId);
            if (character is null)
                throw new UserNotFoundException(userId);
            else
            {
                _context.User.Remove(character);

                if(await _context.SaveChangesAsync() > 0)
                {
                    responce.Data = true;
                    responce.Success = true;
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Message = "user Deleted";
                    return responce;
                };
                responce.Data = false;
                responce.Success = true;
                responce.StatusCode = HttpStatusCode.FailedDependency;
                responce.Message = "User Not Deletd";
                return responce;
            }
        }

        public async Task<ResponseBody<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var responce = new ResponseBody<IEnumerable<UserDto>>();
            try
            {
                var users = await _context.User.AsNoTracking().ToListAsync();

                var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);

                responce.Data = userDtos;
                responce.Success = true;
                responce.StatusCode = HttpStatusCode.OK;
                responce.Message = "";
                return responce;
            }
            catch (Exception ex)
            {
                responce.Success = false;
                responce.StatusCode = HttpStatusCode.InternalServerError;
                responce.Message = "An error occurred while retrieving users.";
            }
            return responce;
        }

        public async Task<ResponseBody<UserDto>> GetUserByIdASync(int id)
        {
            var responce = new ResponseBody<UserDto>();
            try
            {
                var user = await _context.User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        u => u.Id == id
                    );

                if ( user == null )
                {
                    throw new UserNotFoundException(id);
                }
                else
                {
                    var userDto = _mapper.Map<UserDto>(user);

                    responce.Data = userDto;
                    responce.Success = true;
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Message = "";
                    return responce;
                }
            }catch (UserNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserNotFoundException("UserDto not found",ex);
            }
        }

        public async Task<ResponseBody<UserDto>> UpdateUserAsync(UserDto updatedUserDto)
        {
            var responce = new ResponseBody<UserDto>();
            try
            {
                var OldDetial = await _context.User.FirstOrDefaultAsync(
                        c=>c.Email == updatedUserDto.Email
                    );

                if (OldDetial == null)
                {
                    throw new UserNotFoundException();
                }
                else
                {
                    var user=new User();

                    _mapper.Map<UserDto>(user);

                   int i=await _context.SaveChangesAsync();
                    if(i > 0)
                    {
                        responce.Data = updatedUserDto;
                        responce.Success = true;
                        responce.StatusCode = HttpStatusCode.OK;
                        responce.Message = "user registerd";
                        return responce;
                    }
                    else
                    {
                        throw new UserNotUpdatedException();
                    }
                }
            }
            catch (UserNotFoundException)
            {
                throw;
            }
            catch(UserNotUpdatedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UserNotUpdatedException(ex);
            }
        }

        public async Task<int> GetUserIdByEmail(string email)
        {
            var user = await _context.User
                    .AsNoTracking()
                    .FirstOrDefaultAsync(
                        u=>u.Email==email
                    );

            return user.Id;

        }

        public string HashPassword(string password)
        {
            // Hash the password using bcrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<ResponseBody<bool>> ForgetPassword(string Email,string newPassword)
        {
            var responce = new ResponseBody<bool>();
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == Email);

                if (user != null)
                {
                    user.Password = HashPassword(newPassword);

                    await _context.SaveChangesAsync();

                    responce.Data = true;
                    responce.Success = true;
                    responce.StatusCode = HttpStatusCode.OK;
                    responce.Message = "user password Updated";
                    return responce;
                }
                else
                {
                    throw new Exception("User password not updated");
                }
            }
            catch (Exception ex)
            {
                responce.Data = false;
                responce.Success = false;
                responce.StatusCode = HttpStatusCode.NotFound;
                responce.Message = ex.Message;
                return responce;
            }


        }
    }
}
