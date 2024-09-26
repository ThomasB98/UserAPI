using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException():base("User not found") { }
        public UserNotFoundException(int id):base($"User  with {id} not found") { 
        }

        public UserNotFoundException(string message,Exception e) : base(message,e)
        {
        }
    }
}
