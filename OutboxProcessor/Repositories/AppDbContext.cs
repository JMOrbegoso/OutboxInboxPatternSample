using Microsoft.EntityFrameworkCore;

namespace OutboxProcessor.Repositories
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}