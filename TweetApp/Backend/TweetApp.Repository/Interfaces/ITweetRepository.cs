using TweetApp.Repository.Entities;

namespace TweetApp.Repository.Interfaces
{
    public interface ITweetRepository : IRepository<Tweet>
    {
        void Update(Tweet tweet);
    }
}
