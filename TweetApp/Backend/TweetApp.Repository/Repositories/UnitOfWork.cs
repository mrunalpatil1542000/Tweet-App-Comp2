using TweetApp.Repository.Contexts;
using TweetApp.Repository.Interfaces;

namespace TweetApp.Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IUserRepository User { get; private set; }

        public ITweetRepository Tweet { get; private set; }

        public IReplyTweetRepository ReplyTweet { get; private set; }

        public IReactionsRepository Reactions { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            User = new UserRepository(_db);
            Tweet = new TweetRepository(_db);
            ReplyTweet = new ReplyTweetRepository(_db);
            Reactions = new ReactionsRepository(_db);
        }
        public async Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
