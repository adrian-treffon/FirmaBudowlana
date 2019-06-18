using FirmaBudowlana.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FirmaBudowlana.Infrastructure.EF
{
    public class DBContext : DbContext
    {
        
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkerTeam>()
                .HasKey(t => new { t.WorkerID, t.TeamID});

            modelBuilder.Entity<OrderTeam>()
             .HasKey(t => new { t.OrderID, t.TeamID });
        }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderTeam> OrderTeam { get; set; }
        public DbSet<WorkerTeam> WorkerTeam { get; set; }


    }

}

