namespace Receiver.Models
{
    public sealed record InboxMessage(
        string Id,
        DateTime CreatedOn,
        string @Type,
        string Data)
    {
        public DateTime? HandledOn { get; set; } = default!;
    }
}