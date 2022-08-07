namespace Receiver.Models
{
    public sealed record OutboxMessage(
        string Id,
        DateTime CreatedOn,
        string @Type,
        string Data)
    {
        public DateTime? SentOn { get; set; } = default!;
    }
}