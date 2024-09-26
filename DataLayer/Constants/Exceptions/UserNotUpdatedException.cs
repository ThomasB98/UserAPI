using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Exceptions
{
    public class UserNotUpdatedException : Exception
    {
        public UserNotUpdatedException():base("User  not updated") { }

        public UserNotUpdatedException(int id):base(
            $"user updation failed"
            ) { }

        public UserNotUpdatedException(string message,Exception e) : base(message,e) { 
        }

        public UserNotUpdatedException(Exception e) : base("User not updated",e)
        {
        }
    }
}
