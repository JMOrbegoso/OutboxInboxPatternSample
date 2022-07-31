using OutboxProcessor.Repositories;
using Quartz;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace OutboxProcessor.Jobs
{
    [DisallowConcurrentExecution]
    public class SendOutboxJob : IJob
    {
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SendOutboxJob> _logger;

        public SendOutboxJob(
            IOutboxRepository outboxRepository,
            IUnitOfWork unitOfWork,
            ILogger<SendOutboxJob> logger)
        {
            this._outboxRepository = outboxRepository;
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var unsentOutboxes = await this._outboxRepository.GetUnsentOutboxMessages();

            foreach (var outbox in unsentOutboxes)
            {
                this._logger.LogInformation($"Sending {outbox.Id} outbox using RabbitMQ");

                // Sent outbox using RabbitMQ
                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: "outbox",
                                             durable: false,
                                             exclusive: false,
                                             autoDelete: false,
                                             arguments: null);

                        var message = JsonSerializer.Serialize(outbox);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "",
                                             routingKey: "outbox",
                                             basicProperties: null,
                                             body: body);
                    }
                }

                // Update Outbox
                outbox.SentOn = DateTime.UtcNow;
                this._outboxRepository.Update(outbox);
                await this._unitOfWork.SaveChangesAsync();
            }
        }
    }
}