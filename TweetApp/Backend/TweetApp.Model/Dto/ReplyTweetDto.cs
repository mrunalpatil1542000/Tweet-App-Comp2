#nullable disable
namespace TweetApp.Model.Dto
{
    public class ReplyTweetDto
    {
        public int UserId { get; set; }
        public int TweetId { get; set; }
        public string Message { get; set; }
    }
}
