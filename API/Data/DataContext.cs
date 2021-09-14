using API.Entities;
using Microsoft.EntityFrameworkCore;



namespace API.Data
{
    /*
    this class i the entry point of the Entity framework
    */
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
    }
}