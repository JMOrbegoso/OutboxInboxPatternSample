using SenderApi.Models;

namespace SenderApi.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
    }
}