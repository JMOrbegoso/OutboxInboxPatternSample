using Microsoft.EntityFrameworkCore;
using OutboxProcessor.Models;

namespace OutboxProcessor.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}