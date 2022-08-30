using TweetApp.Repository.Entities;

namespace TweetApp.Repository.Interfaces
{
    public interface IReactionsRepository : IRepository<Reaction>
    {
        void Update(Reaction reaction);
    }
}
