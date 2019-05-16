using Microsoft.EntityFrameworkCore;

namespace FirmaBudowlana.Infrastructure.EF
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
        }

       // public DbSet<Car> Cars { get; set; }
    }

}

