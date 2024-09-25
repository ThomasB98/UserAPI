using DataLayer.Constants.dbConnection;
using DataLayer.Interface;
using ModelLayer.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Constants.Exceptions;

namespace DataLayer.Repository
{
    public class UserRepository : IUser
    {
        private readonly DataContext _context;

        public UserRepository(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<User> CreateUserAsync(User user)
        {
            _context.User.Add(user);
            int changes= await _context.SaveChangesAsync();
            if (changes > 0)
            {
                return user;
            }
            else
            {
                throw new UserNotSavedException("");
            }
        }

        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User> GetUserByIdASync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<User> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}
