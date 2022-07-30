using SenderApi.Models;

namespace SenderApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appContext;

        public UserRepository(AppDbContext appContext)
        {
            this._appContext = appContext;
        }

        public void Add(User user)
        {
            this._appContext.Users.Add(user);
        }
    }
}