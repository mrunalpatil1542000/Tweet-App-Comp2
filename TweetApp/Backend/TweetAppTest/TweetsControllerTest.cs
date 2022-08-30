﻿using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using TweetApp.Api.Controllers;
using TweetApp.Model.Dto;
using TweetApp.Repository.Entities;
using TweetApp.Repository.Interfaces;
using TweetApp.Service.Services.Interfaces;

namespace TweetAppTest
{
    [TestClass]
    public class TweetsControllerTest
    {
        private Mock<ILogger<TweetsController>> _logger;
        private Mock<IServices> _tweetService;
        private Fixture _fixture;
        private TweetsController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        //private Mock<ITweetRepository> _mockTweetRepo;
        // private readonly TweetAppService _tweetAppService;

        public TweetsControllerTest()
        {
            _logger = new Mock<ILogger<TweetsController>>();
            _tweetService = new Mock<IServices>();
            _fixture = new Fixture();
            _unitOfWork = new Mock<IUnitOfWork>();

        }

        [TestMethod]
        public async Task ViewAllTweets_Test()
        {

            var tweetList = _fixture.CreateMany<Tweet>(3);

            _unitOfWork.Setup(u => u.Tweet.GetAllAsync(It.IsAny<Expression<Func<Tweet, bool>>>(), null)).ReturnsAsync(tweetList);

            _controller = new TweetsController(_tweetService.Object, _logger.Object);
            var result = await _controller.ViewAllTweets();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task PostTweet_Test()
        {

            var tweetList = _fixture.CreateMany<Tweet>(3);
            var tweetListDto = _fixture.CreateMany<TweetCreateDto>(3) as TweetCreateDto;

            _unitOfWork.Setup(u => u.Tweet.AddAsync(It.IsAny<Tweet>()));

            _controller = new TweetsController(_tweetService.Object, _logger.Object);
            var result = await _controller.PostTweet(It.IsAny<string>(), tweetListDto);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task UpdateTweet_Test()
        {

            var tweetList = _fixture.CreateMany<Tweet>(3);
            var tweetListDto = _fixture.CreateMany<TweetCreateDto>(3) as TweetCreateDto;

            _unitOfWork.Setup(u => u.Tweet.Update(It.IsAny<Tweet>()));

            _controller = new TweetsController(_tweetService.Object, _logger.Object);
            var result = await _controller.UpdateTweet(It.IsAny<string>(), It.IsAny<int>(), tweetListDto);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task DeleteTweet_Test()
        {

            _unitOfWork.Setup(u => u.Tweet.Remove(It.IsAny<Tweet>()));

            _controller = new TweetsController(_tweetService.Object, _logger.Object);
            var result = await _controller.DeleteTweet(It.IsAny<string>(), It.IsAny<int>());

            Assert.IsNotNull(result);
        }
    }
}
//Checking service methods and controller methods
//[TestMethod]
//public void ViewAllTweets_Test()
//{

//    var tweetList = _fixture.CreateMany<TweetDetailsDto>(3);

//    _tweetService.Setup(service => service.TweetService.GetAllTweets()).Returns(Task.FromResult(tweetList));

//    _controller = new TweetsController(_tweetService.Object, _logger.Object);
//    var result = _controller.ViewAllTweets();

//    Assert.IsNotNull(result);
//}

