using Microsoft.EntityFrameworkCore;
using Receiver.Repositories;

namespace Receiver
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddDbContext<AppDbContext>(options =>
                    {
                        var connectionString = configuration.GetConnectionString("Database");
                        options.UseSqlServer(connectionString);
                    });
                });
        }
    }
}