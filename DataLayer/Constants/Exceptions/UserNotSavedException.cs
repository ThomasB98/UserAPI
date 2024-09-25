using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Exceptions
{
    public class UserNotSavedException : Exception
    {
        // Default constructor
        public UserNotSavedException()
            : base("The user could not be saved.")
        {
        }

        // Constructor that accepts a custom message
        public UserNotSavedException(string message)
            : base(message)
        {
        }
    }
}
