namespace TweetApp.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        ITweetRepository Tweet { get; }
        IReplyTweetRepository ReplyTweet { get; }
        IReactionsRepository Reactions { get; }
        Task Save();
    }
}
