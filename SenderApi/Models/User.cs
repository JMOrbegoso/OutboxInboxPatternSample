namespace SenderApi.Models
{
    public sealed record User(
        string Id,
        string Email,
        string Password)
    {
    }
}