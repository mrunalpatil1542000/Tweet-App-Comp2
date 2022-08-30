using TweetApp.Repository.Entities;
#nullable disable
namespace TweetApp.Model.Dto
{
    public class ReactionResponse
    {
        public int Id { get; set; }
        public int TweetId { get; set; }
        public int UserId { get; set; }
        public UserDetailsDto User { get; set; }
        public ReactionTypes Reactions { get; set; }
    }
}
