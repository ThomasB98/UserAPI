using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Exceptions
{
    internal class AddressNotSavedException : Exception
    {
        public AddressNotSavedException():base("Address not saved")
        { }

        public AddressNotSavedException(int id) : base($"Address not saved for id {id}")
        { }
    }
}
