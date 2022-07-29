using Microsoft.EntityFrameworkCore;
using SenderApi.Models;

namespace SenderApi.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}