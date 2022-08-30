using Confluent.Kafka;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TweetApp.Model.Api;
using TweetApp.Model.Dto;
using TweetApp.Model.Input;
using TweetApp.Service.Services.Interfaces;

namespace TweetApp.Api.Controllers
{
    [Route("api/v{version:apiVersion}/tweets")]
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    public class UsersController : ControllerBase
    {
        private readonly IServices _services;
        protected ResponseDto _response;
        private readonly ILogger<UsersController> _logger;
        // private readonly IRabbitMQMessageSender _messageSender;
        public UsersController(IServices services, ILogger<UsersController> logger)//,IRabbitMQMessageSender messageSender)
        {
            _services = services;
            _response = new ResponseDto();
            _logger = logger;
          //  _messageSender = messageSender;
        }
        [HttpGet("users/all")]
        public async Task<object> GetAllUsers()
        {
            try
            {
                _logger.LogInformation("Getting all users");
                var userList = await _services.UserService.GetAllUsers();
                _response.DisplayMessage = "User list retrieved successfully";
                _response.Result = userList;
                _logger.LogInformation("User list retrieved successfully");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error cannot find the list";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] UserDto userDto)
        {
            try
            {
                _logger.LogInformation("Registering user");
                if (!ModelState.IsValid)
                    throw new Exception("Invalid data");
                if (!await _services.UserService.IsUniqueUser(userDto.Email))
                {
                    var user = await _services.UserService.Register(userDto);
                    _response.Result = user;
                    _response.DisplayMessage = "User registered successfully";

                    //Kafka part
                    using (var producer =
               new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
                    {
                        try
                        {
                            Console.WriteLine(producer.ProduceAsync("tweetapp_topic", new Message<Null, string> { Value = userDto.Email + " registered!" })
                                .GetAwaiter()
                                .GetResult());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Oops, something went wrong: {e}");
                        }
                    }

                }
                else
                    throw new Exception("Username already exists");
                _logger.LogInformation("User registered");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "Error cannot register";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
           // _messageSender.Publish(_response.DisplayMessage);
            return _response;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            try
            {
                _logger.LogInformation("User Logging");
                var user = await _services.UserService.Authenticate(model.Username, model.Password);

                if (user == null)
                    throw new Exception("Invalid username or password");

                var token = _services.JwtService.GenerateToken(model);
                _response.Result = user;
                _response.DisplayMessage = "Login successful for " + model.Username;
                _response.Token=token;
                _logger.LogInformation("User Logged in");

                // Kafka part
                using (var producer =
                new ProducerBuilder<Null, string>(new ProducerConfig { BootstrapServers = "localhost:9092" }).Build())
                {
                    try
                    {
                        Console.WriteLine(producer.ProduceAsync("tweetapp_topic", new Message<Null, string> { Value = model.Username + " logged in!" })
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
                _response.DisplayMessage = "Something went wrong while logging in.";
                _response.ErrorMessages = new List<string> { ex.Message };
            }
            //_messageSender.Publish(_response.DisplayMessage);

            return _response;
        }

        [HttpPatch("{username}/forgot")]
        [AllowAnonymous]
        public async Task<object> ForgotPassword(string username, [FromBody] string password)
        {
            try
            {
                _logger.LogInformation("changing password");
                var status = await _services.UserService.ResetPassword(username, password);

                if (status == false)
                    throw new Exception("Error while resetting password");

                _response.DisplayMessage = "Password reset successfully";
                _response.Result = status;
                _logger.LogInformation("Password changed");
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

        [HttpGet("search/{username}")]
        public async Task<object> SearchUsername(string username)
        {
            try
            {
                _logger.LogInformation("Searching username");
                var users=await _services.UserService.FindUsersByUsername(username);
                _response.Result = users;
                
                if(users==null)
                {
                    _response.DisplayMessage = "No users found";
                }
                else
                {
                    _response.DisplayMessage = "Users retrieved successfully";
                }
                _logger.LogInformation("User found");
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

        [HttpGet("currentuser")]
        public async Task<object> GetCurrentUser()
        {
            try
            {
                _logger.LogInformation("Getting logged in user");
                var username = User.FindFirstValue(ClaimTypes.Email);
                var user = await _services.UserService.FindByUsername(username);
                _response.Result = user;
                _logger.LogInformation("Got current user");
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.DisplayMessage = "You are not logged in. Please log in again";
                _response.ErrorMessages=new List<string> { ex.Message};
            }
            return _response;
        }
    }
}
