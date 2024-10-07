using AutoMapper;
using DataLayer.Constants.dbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.Entity;

namespace DataLayer.Repository
{
    public class LoginRepository
    {
        private readonly DataContext _context;

        public LoginRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Login(string Email, string password)
        {
            var user= await _context.User.FirstOrDefaultAsync(u => u.Email == Email);

            if (user != null && Hashing.VerifyPassword(password, user.Password))
            {
                return user;
            }

            // If no user is found or password does not match, return false
            return null;
        }

        static class Hashing
        {

            public static bool VerifyPassword(string password, string hashedPassword)
            {
                // Verify the hashed password
                return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
            }
        }
    }
}
