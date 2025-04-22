using Microsoft.EntityFrameworkCore;

namespace RedressAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<DbContext> options) : base(options) { }
    }
}
