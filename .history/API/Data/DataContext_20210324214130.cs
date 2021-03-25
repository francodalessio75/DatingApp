using Microsoft.EntityFrameworkCore;



namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppAuser> MyProperty { get; set; }
    }
}