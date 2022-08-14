using Microsoft.EntityFrameworkCore;
using Quartz;
using Receiver.Jobs;
using Receiver.Repositories;
using Receiver.Workers;

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

                    services.AddScoped<IInboxRepository, InboxRepository>();
                    services.AddScoped<IUnitOfWork, UnitOfWork>();

                    services.AddQuartz(quarz =>
                    {
                        quarz.UseMicrosoftDependencyInjectionJobFactory();

                        quarz.ScheduleJob<SendEmailJob>(trigger =>
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

                    services.AddHostedService<ReceiveMessageWorker>();
                });
        }
    }
}