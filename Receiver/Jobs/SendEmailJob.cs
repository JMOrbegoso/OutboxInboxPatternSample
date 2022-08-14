using Quartz;
using Receiver.Repositories;

namespace Receiver.Jobs
{
    [DisallowConcurrentExecution]
    public class SendEmailJob : IJob
    {
        private readonly IInboxRepository _inboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<SendEmailJob> _logger;

        public SendEmailJob(
            IInboxRepository inboxRepository,
            IUnitOfWork unitOfWork,
            ILogger<SendEmailJob> logger)
        {
            this._inboxRepository = inboxRepository;
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var unhandledInboxes = await this._inboxRepository.GetUnhandled();

            foreach (var inbox in unhandledInboxes)
            {
                this._logger.LogInformation($"Handling {inbox.Id} inbox");

                // Send email
                await Task.Delay(1000);

                // Update Inbox
                inbox.HandledOn = DateTime.UtcNow;
                this._inboxRepository.Update(inbox);
                await this._unitOfWork.SaveChangesAsync();
            }
        }
    }
}