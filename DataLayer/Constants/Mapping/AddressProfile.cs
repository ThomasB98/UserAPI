using AutoMapper;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.Mapping
{
     public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<Address,AddressProfile>();
            CreateMap<AddressProfile, Address>();
        }
    }
}
