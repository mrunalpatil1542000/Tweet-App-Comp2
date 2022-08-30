using TweetApp.Repository.Contexts;
using TweetApp.Repository.Entities;
using TweetApp.Repository.Interfaces;

namespace TweetApp.Repository.Repositories
{
    public class ReplyTweetRepository : Repository<ReplyTweet>, IReplyTweetRepository
    {
        private readonly ApplicationDbContext _db;
        public ReplyTweetRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
