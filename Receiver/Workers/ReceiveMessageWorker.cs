using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Receiver.Models;
using Receiver.Repositories;
using System.Text;
using System.Text.Json;

namespace Receiver.Workers
{
    public class ReceiveMessageWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReceiveMessageWorker> _logger;
        private IConnection _connection;
        private IModel _channel;

        public ReceiveMessageWorker(
            IServiceProvider serviceProvider,
            ILogger<ReceiveMessageWorker> logger)
        {
            this._serviceProvider = serviceProvider;
            this._logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                DispatchConsumersAsync = true
            };

            this._logger.LogInformation($"Creating RabbitMQ connection.");

            this._connection = factory.CreateConnection();

            this._channel = this._connection.CreateModel();

            this._channel.QueueDeclare(queue: "outbox", durable: false, exclusive: false, autoDelete: false, arguments: null);

            this._channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            this._logger.LogInformation($"Outbox queue is waiting for messages.");

            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            this._connection.Close();
            this._logger.LogInformation("RabbitMQ connection is closed.");

            await base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(this._channel);

            consumer.Received += async (model, args) =>
            {
                this._logger.LogInformation($"Processing new message.");

                byte[] body = args.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                OutboxMessage? outbox = JsonSerializer.Deserialize<OutboxMessage>(message);
                if (outbox == null)
                    return;

                var inbox = new InboxMessage(outbox.Id, outbox.CreatedOn, outbox.Type, outbox.Data);

                using (IServiceScope scope = this._serviceProvider.CreateScope())
                {
                    IInboxRepository inboxRepository = scope.ServiceProvider.GetRequiredService<IInboxRepository>();
                    IUnitOfWork unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    try
                    {
                        inboxRepository.Add(inbox);
                        await unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }

                this._channel.BasicAck(deliveryTag: args.DeliveryTag, multiple: false);
            };

            this._channel.BasicConsume(queue: "outbox", autoAck: false, consumer: consumer);

            await Task.CompletedTask;
        }
    }
}