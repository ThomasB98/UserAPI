using AutoMapper;
using DataLayer.Constants.dbConnection;
using DataLayer.Constants.Exceptions;
using DataLayer.Constants.ResponceEntity;
using DataLayer.Interface;
using ModelLayer.Model.Dto;
using ModelLayer.Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repository
{
    public class AddressRepository : IAddress
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
       

        public AddressRepository(DataContext dataContext, IMapper mapper)
        {
            _context = dataContext;
            _mapper = mapper;
        }

        public async Task<bool> CreateAddressAsync(AddressDto address)
        {
            try
            {
                var addres = _mapper.Map<Address>(address);

                await _context.Address.AddAsync(addres);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }


        public Task<ResponseBody<AddressDto>> GetAddressByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseBody<IEnumerable<AddressDto>>> GetAllAddressesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAddressAsync(AddressDto address,int id)
        {
            var adr=_mapper.Map<Address>(address);
            _context.Address.Update(adr);
            if (await _context.SaveChangesAsync() > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
