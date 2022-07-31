using Microsoft.AspNetCore.Mvc;
using SenderApi.Commands;
using SenderApi.Models;
using SenderApi.Repositories;
using System.Text.Json;

namespace SenderApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IOutboxRepository _outboxRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            IUserRepository userRepository,
            IOutboxRepository outboxRepository,
            IUnitOfWork unitOfWork,
            ILogger<UsersController> logger)
        {
            this._userRepository = userRepository;
            this._outboxRepository = outboxRepository;
            this._unitOfWork = unitOfWork;
            this._logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserDto dto)
        {
            var user = new User(Guid.NewGuid().ToString(), dto.email, dto.password);
            var command = new SendEmailCommand(Guid.NewGuid().ToString(), user.Email, "Welcome");
            var outbox = new OutboxMessage(command.Id, DateTime.UtcNow, nameof(SendEmailCommand), JsonSerializer.Serialize(command));

            this._userRepository.Add(user);
            this._outboxRepository.Add(outbox);

            await this._unitOfWork.SaveChangesAsync();

            return Ok();
        }

        public record UserDto(string email, string password);
    }
}