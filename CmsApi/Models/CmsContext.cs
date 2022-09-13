using CmsClassLibrary;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CmsApi.Models
{
    public class CmsContext : DbContext
    {
        public CmsContext([NotNullAttribute]
            DbContextOptions options) : base(options)
        {

        }
        public DbSet<Cargo> Cargo { get; set; }
        public DbSet<CargoOrderDetail> CargoOrderDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Admin> Admin { get; set; }
    }
}
