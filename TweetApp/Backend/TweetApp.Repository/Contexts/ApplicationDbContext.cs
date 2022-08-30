using Microsoft.EntityFrameworkCore;
using TweetApp.Repository.Entities;

#nullable disable
namespace TweetApp.Repository.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<ReplyTweet> ReplyTweets { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
    }
}
