using Microsoft.EntityFrameworkCore;
using ModelLayer.Model.Address;
using ModelLayer.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Constants.dbConnection
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<User> User => Set<User>();
        public DbSet<Address> Address => Set<Address>();
    }
}
