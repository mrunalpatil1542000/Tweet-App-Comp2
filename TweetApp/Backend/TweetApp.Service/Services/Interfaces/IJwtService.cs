using TweetApp.Model.Input;

namespace TweetApp.Service.Services.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(LoginDto model);
    }
}
