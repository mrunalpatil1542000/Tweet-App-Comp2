using AutoMapper;
using TweetApp.Model.Dto;
using TweetApp.Repository.Entities;
using TweetApp.Repository.Interfaces;
using TweetApp.Service.Services.Interfaces;

#nullable disable
namespace TweetApp.Service.Services
{
    public class TweetService : ITweetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TweetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<int> CountLikes(int id)
        {
            var list = await _unitOfWork.Reactions.GetAllAsync(x => x.TweetId == id);
            return list.Count();
        }

        public async Task<bool> DeleteTweet(int id, string username)
        {
            var tweet = await _unitOfWork.Tweet.GetFirstOrDefaultAsync(x => x.Id == id &&
            x.User.Email == username, includeProperties: "User");

            if (tweet == null)
                return false;

            _unitOfWork.Tweet.Remove(tweet);
            await _unitOfWork.Save();
            return true;

        }

        public async Task<IEnumerable<TweetDetailsDto>> GetAllTweets()
        {
            var tweetList = await _unitOfWork.Tweet.GetAllAsync(includeProperties: "User");

            var tweets = new List<TweetDetailsDto>();

            foreach (var tweet in tweetList)
            {
                tweets.Add(_mapper.Map<TweetDetailsDto>(tweet));
            }

            return tweets;
        }

        public async Task<TweetDetailsDto> GetATweet(int id)
        {
            var tweet = await _unitOfWork.Tweet.GetFirstOrDefaultAsync(x => x.Id == id, includeProperties: "User");
            return _mapper.Map<TweetDetailsDto>(tweet);
        }

        public async Task<IEnumerable<TweetDetailsDto>> GetTweetsByUsername(string username)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == username);

            var tweetList = await _unitOfWork.Tweet.GetAllAsync(x => x.UserId == user.LoginId, includeProperties: "User");

            var tweets = new List<TweetDetailsDto>();

            foreach (var tweet in tweetList)
            {
                tweets.Add(_mapper.Map<TweetDetailsDto>(tweet));
            }

            return tweets;
        }

        public async Task<int> LikeTweet(string username, int id)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == username);
            if (user == null)
                return 0;

            var reac=await _unitOfWork.Reactions
                .GetFirstOrDefaultAsync(x=>x.TweetId == id && x.UserId==user.LoginId);

            if(reac== null)
            {
                var reaction = new Reaction
                {
                    TweetId = id,
                    UserId = user.LoginId,
                    Reactions = ReactionTypes.LIKE
                };
                await _unitOfWork.Reactions.AddAsync(reaction);
                await _unitOfWork.Save();
                return 1;
            }
            else
            {
                 _unitOfWork.Reactions.Remove(reac);
                await _unitOfWork.Save();
                return 2;
            }
        }

        public async Task<TweetDetailsDto> PostTweet(string username, TweetCreateDto tweetDto)
        {
            var tweet = _mapper.Map<Tweet>(tweetDto);
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == username);
            tweet.UserId = user.LoginId;
            await _unitOfWork.Tweet.AddAsync(tweet);
            await _unitOfWork.Save();
            return _mapper.Map<TweetDetailsDto>(tweet);
        }

        public async Task<ReplyResponse> ReplyTweet(string username, int id, string message)
        {
            var user = await _unitOfWork.User.GetFirstOrDefaultAsync(x => x.Email == username);

            ReplyTweetDto replyDto = new()
            {
                UserId = user.LoginId,
                TweetId = id,
                Message = message
            };

            var reply = _mapper.Map<ReplyTweet>(replyDto);
            await _unitOfWork.ReplyTweet.AddAsync(reply);
            await _unitOfWork.Save();


            var response = await _unitOfWork.ReplyTweet
                .GetFirstOrDefaultAsync(x => x.UserId == user.LoginId && x.TweetId == id &&x.Message==message);
            return _mapper.Map<ReplyResponse>(response);
        }

        public async Task<TweetDetailsDto> UpdateTweet(int id, string username, TweetCreateDto tweetDto)
        {
            var tweet = await _unitOfWork.Tweet.GetFirstOrDefaultAsync(x => x.Id == id && x.User.Email == username, includeProperties: "User");
            tweet.Tag = tweetDto.Tag;
            tweet.Subject = tweetDto.Subject;
            _unitOfWork.Tweet.Update(tweet);
            await _unitOfWork.Save();
            return _mapper.Map<TweetDetailsDto>(tweet);
        }

        public async Task<IEnumerable<ReactionResponse>> GetReactionsList()
        {
            var reaction = await _unitOfWork.Reactions.GetAllAsync( includeProperties: "User");
            
            List<ReactionResponse> responses = new();
            
            foreach (var item in reaction)
            {
                responses.Add(_mapper.Map<ReactionResponse>(item));
            }
            
            return responses;
        }

        public async Task<IEnumerable<ReplyResponse>> GetRepliesList()
        {
            var replies = await _unitOfWork.ReplyTweet.GetAllAsync(includeProperties: "User");
            replies = replies.OrderByDescending(x => x.DatePosted);

            var responseReply=new List<ReplyResponse>();

            foreach (var item in replies)
            {
                responseReply.Add(_mapper.Map<ReplyResponse>(item));
            }
            return responseReply;
        }
    }
}
