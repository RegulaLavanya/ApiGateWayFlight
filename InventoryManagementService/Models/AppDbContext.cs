using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryManagementService.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Airlines> Airlines { get; set; }
        public DbSet<Flights> Flights { get; set; }
        public DbSet<FlightSchedules> FlightSchedules { get; set; }
        public DbSet<Discounts> Discounts { get; set; }
    }
}
