using TweetApp.Repository.Contexts;
using TweetApp.Repository.Entities;
using TweetApp.Repository.Interfaces;

namespace TweetApp.Repository.Repositories
{
    public class ReactionsRepository : Repository<Reaction>, IReactionsRepository
    {
        private readonly ApplicationDbContext _db;
        public ReactionsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Reaction reaction)
        {
            _db.Reactions.Update(reaction);
        }
    }
}
