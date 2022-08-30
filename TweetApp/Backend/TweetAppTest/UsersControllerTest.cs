using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
using TweetApp.Api.Controllers;
using TweetApp.Model.Dto;
using TweetApp.Model.Input;
using TweetApp.Repository.Entities;
using TweetApp.Repository.Interfaces;
using TweetApp.Service.Services.Interfaces;

namespace TweetAppTest
{
    [TestClass]
    public class UsersControllerTest
    {
        private Mock<ILogger<UsersController>> _logger;
        private Mock<IServices> _tweetService;
        private Fixture _fixture;
        private UsersController _controller;
        private Mock<IUnitOfWork> _unitOfWork;
        //private Mock<ITweetRepository> _mockTweetRepo;
        // private readonly TweetAppService _tweetAppService;

        public UsersControllerTest()
        {
            _logger = new Mock<ILogger<UsersController>>();
            _tweetService = new Mock<IServices>();
            _fixture = new Fixture();
            _unitOfWork = new Mock<IUnitOfWork>();

        }

        [TestMethod]
        public async Task GetAllUsers_Test()
        {

            var userList = _fixture.CreateMany<User>(3);

            _unitOfWork.Setup(u => u.User.GetAllAsync(It.IsAny<Expression<Func<User, bool>>>(), null)).ReturnsAsync(userList);

            _controller = new UsersController(_tweetService.Object, _logger.Object);
            var result = await _controller.GetAllUsers();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Register_Test()
        {

            var userList = _fixture.CreateMany<User>(3);
            var userListDto = _fixture.CreateMany<UserDto>(3) as UserDto;

            _unitOfWork.Setup(u => u.User.AddAsync(It.IsAny<User>()));

            _controller = new UsersController(_tweetService.Object, _logger.Object);
            var result = await _controller.Register(userListDto);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Login_Test()
        {

            var userList = _fixture.CreateMany<User>(3);
            var userLoginDto = _fixture.CreateMany<LoginDto>(3) as LoginDto;

            _unitOfWork.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null));

            _controller = new UsersController(_tweetService.Object, _logger.Object);
            var result = await _controller.Login(userLoginDto);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task ForgotPassword_Test()
        {

            var userList = _fixture.CreateMany<User>(3) as User;

            _unitOfWork.Setup(u => u.User.Update(userList));

            _controller = new UsersController(_tweetService.Object, _logger.Object);
            var result = await _controller.ForgotPassword(It.IsAny<string>(), It.IsAny<string>());

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task SearchUsername_Test()
        { 
            var userList = _fixture.CreateMany<User>(3) as User;

            _unitOfWork.Setup(u => u.User.GetFirstOrDefaultAsync(It.IsAny<Expression<Func<User, bool>>>(), null));

            _controller = new UsersController(_tweetService.Object, _logger.Object);
            var result = await _controller.SearchUsername(It.IsAny<string>());

            Assert.IsNotNull(result);
        }
    }
}
