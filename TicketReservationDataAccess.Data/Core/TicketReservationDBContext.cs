using TicketReservationDataAccess.Data.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TicketReservationDataAccess.Data.Core
{
    public class TicketReservationDBContext:DbContext
    {
        public  TicketReservationDBContext(DbContextOptions<TicketReservationDBContext> dbContext) : base(dbContext) 
        { 


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {


            modelBuilder.Entity<Schedule>()
                .Property(v => v.ModeOfTransport)
                .HasConversion<int>();

            
        }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Terminal> Terminals { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<Destination> Destinations { get; set; }
    }
}
