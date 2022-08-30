using TweetApp.Repository.Contexts;
using TweetApp.Repository.Entities;
using TweetApp.Repository.Interfaces;

namespace TweetApp.Repository.Repositories
{
    public class TweetRepository : Repository<Tweet>, ITweetRepository
    {
        private readonly ApplicationDbContext _db;
        public TweetRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Tweet tweet)
        {
            _db.Tweets.Update(tweet);
        }
    }
}
