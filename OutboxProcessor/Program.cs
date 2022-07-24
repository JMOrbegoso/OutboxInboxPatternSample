using Microsoft.EntityFrameworkCore;
using OutboxProcessor.Repositories;

namespace OutboxProcessor
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

                    services.AddHostedService<Worker>();
                });
        }
    }
}