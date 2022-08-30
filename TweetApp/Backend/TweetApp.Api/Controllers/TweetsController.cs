using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TweetApp.Model.Api;
using TweetApp.Model.Dto;
using TweetApp.Service.Services.Interfaces;

namespace TweetApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/tweets")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class TweetsController : ControllerBase
    {
        private readonly IServices _services;
        protected ResponseDto _response;
        private readonly ILogger<TweetsController> _logger;
        //  private readonly IRabbitMQMessageSender _messageSender;
        public TweetsController(IServices services, ILogger<TweetsController> logger)//, IRabbitMQMessageSender messageSender)
        {
            _services = services;
            _response = new ResponseDto();
            _logger = logger;
            // _messageSender = messageSender;
        }

        [HttpPost("{username}/add")]
        public async Task<object> PostTweet([FromRoute] string username, [FromBody] TweetCreateDto tweetDto)
        {
            try
            {
                _logger.LogInformation("Posting tweet.");
                var tweet = await _services.TweetService.PostTweet(username, tweetDto);
                _response.Result = tweet;
                _response.DisplayMessage = "Tweet posted successfully";
                _logger.LogInformation("Tweet posted successfully.");
                //Kafka part
                using (var producer =
              new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
                {
                    try
                    {
                        Console.WriteLine(producer.ProduceAsync("tweetapp_topic", new Message<Null, string> { Value = username + " tweet posted!" })
                            .GetAwaiter()
                            .GetResult());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Oops, something went wrong: {e}");
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong!";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpGet("all")]
        public async Task<object> ViewAllTweets()
        {
            try
            {
                _logger.LogInformation("Viewing all tweets.");
                var tweets = await _services.TweetService.GetAllTweets();
                _response.Result = tweets;
                _response.DisplayMessage = "List of tweets fetched successfully";
                _logger.LogInformation("Returned all tweets.");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong!";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpGet("{username}")]
        public async Task<object> GetAllTweetsOfUser(string username)
        {
            try
            {
                _logger.LogInformation("Getting user's tweets.");
                var tweetList = await _services.TweetService.GetTweetsByUsername(username);
                _response.Result = tweetList;
                _logger.LogInformation("Got user's tweets.");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong!";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpPut("{username}/update/{id}")]
        public async Task<object> UpdateTweet([FromRoute] string username, [FromRoute] int id, [FromBody] TweetCreateDto tweetDto)
        {
            try
            {
                _logger.LogInformation("Updating tweet.");
                var status = await _services.TweetService.UpdateTweet(id, username, tweetDto);
                if (status != null)
                {
                    _response.Result = status;
                    _response.DisplayMessage = "Tweet updated successfully";
                    _logger.LogInformation("Tweet updated.");
                }
                else
                    throw new Exception("Something wrong!!!");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong while updating tweet! Please try again later.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpDelete("{username}/delete/{id}")]
        public async Task<object> DeleteTweet([FromRoute] string username, [FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Deleting tweet.");
                var status = await _services.TweetService.DeleteTweet(id, username);
                if (status)
                {
                    _response.Result = true;
                    _response.DisplayMessage = "Tweet deleted successfully";
                    _logger.LogInformation("Tweet Deleted.");
                }
                else
                    throw new Exception("Something wrong!!!");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong while deleting tweet! Please try again later.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            //_messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpPost("{username}/reply/{id}")]
        public async Task<object> ReplyTweet([FromRoute] string username, [FromRoute] int id
            , [FromBody] Body body)
        {
            try
            {
                _logger.LogInformation("Replying to tweet.");
                var replyObj = await _services.TweetService.ReplyTweet(username, id, body.Message);
                _response.Result = replyObj;
                _response.DisplayMessage = "Replied Successfully";
                _logger.LogInformation("Replied to tweet.");

                //Kafka part
                using (var producer =
              new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
                {
                    try
                    {
                        Console.WriteLine(producer.ProduceAsync("tweetapp_topic", new Message<Null, string> { Value = username + " tweet replied!" })
                            .GetAwaiter()
                            .GetResult());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Oops, something went wrong: {e}");
                    }
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong while replying the tweet! Please try again later.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            //_messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpPost("{username}/like/{id}")]
        public async Task<object> LikeTweet([FromRoute] string username, [FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Liking tweet.");
                var status = await _services.TweetService.LikeTweet(username, id);
                _response.Result = status;
                _response.DisplayMessage = "Tweet liked successfully";
                _logger.LogInformation("Tweet liked.");

                //Kafka part
                using (var producer =
               new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
                {
                    try
                    {
                        Console.WriteLine(producer.ProduceAsync("tweetapp_topic", new Message<Null, string> { Value = username + " tweet liked!" })
                            .GetAwaiter()
                            .GetResult());
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Oops, something went wrong: {e}");
                    }
                }
            }
            catch (Exception ex)
            {
                _response.Result = false;
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong while replying the tweet! Please try again later.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpGet("details/{id}")]
        public async Task<object> GetATweet(int id)
        {
            try
            {
                _logger.LogInformation("Getting tweet using Id.");
                var tweet = await _services.TweetService.GetATweet(id);
                if (tweet == null)
                    throw new Exception("No tweets found");
                _response.Result = tweet;
                _response.DisplayMessage = "Tweet fetched successfully";
                _logger.LogInformation("Tweet got.");
            }
            catch (Exception ex)
            {
                _response.Result = false;
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong while displaying the tweet! Please try again later.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("like/{id}")]
        public async Task<object> GetLikeCountOfTweet(int id)
        {
            try
            {
                var likes = await _services.TweetService.CountLikes(id);
                _response.Result = likes;
                _response.DisplayMessage = "Tweet Count fetched successfully";
            }
            catch (Exception ex)
            {
                _response.Result = false;
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("reactions")]
        public async Task<object> GetAllReactions()
        {
            try
            {
                _logger.LogInformation("Getting all reactions.");
                var responses = await _services.TweetService.GetReactionsList();
                _response.Result = responses;
                _response.DisplayMessage = "Reactions fetched";
                _logger.LogInformation("Got all reactions.");
            }
            catch (Exception ex)
            {
                _response.Result = false;
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }

        [HttpGet("replies")]
        public async Task<object> GetAllReplies()
        {
            try
            {
                _logger.LogInformation("Getting all replies.");
                var responses = await _services.TweetService.GetRepliesList();
                _response.Result = responses;
                _response.DisplayMessage = "Replies fetched";
                _logger.LogInformation("Got all replies.");
            }
            catch (Exception ex)
            {
                _response.Result = false;
                _response.IsSuccess = false;
                _response.DisplayMessage = "Something went wrong.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            return _response;
        }
    }
}
