using Microsoft.EntityFrameworkCore;
using OutboxProcessor.Jobs;
using OutboxProcessor.Repositories;
using Quartz;

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

                    services.AddScoped<IOutboxRepository, OutboxRepository>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();

                    services.AddQuartz(quarz =>
                    {
                        quarz.UseMicrosoftDependencyInjectionJobFactory();

                        quarz.ScheduleJob<SendOutboxJob>(trigger =>
                        {
                            trigger.WithSimpleSchedule(x =>
                            {
                                x.WithIntervalInSeconds(3)
                                 .RepeatForever();
                            }).StartNow();
                        });
                    });

                    services.AddQuartzHostedService(quarz =>
                    {
                        quarz.WaitForJobsToComplete = true;
                    });
                });
        }
    }
}