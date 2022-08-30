using TweetApp.Repository.Entities;

namespace TweetApp.Repository.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        void Update(User user);
    }
}
