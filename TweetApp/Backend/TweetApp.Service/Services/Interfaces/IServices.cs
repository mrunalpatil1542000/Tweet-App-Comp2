namespace TweetApp.Service.Services.Interfaces
{
    public interface IServices
    {
        IUserService UserService { get; }
        IJwtService JwtService { get; }
        ITweetService TweetService { get; }
    }
}
