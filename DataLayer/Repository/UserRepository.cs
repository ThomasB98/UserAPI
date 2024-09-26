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
using Azure;

namespace DataLayer.Repository
{
    public class UserRepository : IUser
    {   
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext dataContext,IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;
        }


        public async Task<ResponseBody<UserDto>> CreateUserAsync(UserDto userDto)
        {
            var responce=new ResponseBody<UserDto>();
            var user=_mapper.Map<User>(userDto);

            user.CreatedDate = DateTime.Now;
            _context.User.Add(user);
            int changes= await _context.SaveChangesAsync();
            if (changes > 0)
            {
                responce.Data = userDto;
                responce.Success = true;
                responce.StatusCode=HttpStatusCode.OK;
                responce.Message = "user registerd";
                return responce;
            }
            else
            {
                throw new UserNotSavedException();
            }
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

    }
}
