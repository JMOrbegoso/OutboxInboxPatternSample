using Microsoft.EntityFrameworkCore;
using Receiver.Models;

namespace Receiver.Repositories
{
    public class AppDbContext : DbContext
    {
        public DbSet<InboxMessage> InboxMessages { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}