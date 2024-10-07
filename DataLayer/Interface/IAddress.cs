using DataLayer.Constants.ResponceEntity;
using ModelLayer.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Interface
{
    public interface IAddress
    {
        Task<ResponseBody<AddressDto>> GetAddressByIdAsync(int id);
        Task<bool> CreateAddressAsync(AddressDto address);
        Task<bool> UpdateAddressAsync(AddressDto address,int id);
        Task<ResponseBody<IEnumerable<AddressDto>>> GetAllAddressesAsync();
    }
}
