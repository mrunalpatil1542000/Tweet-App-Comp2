using AutoMapper;
using TweetApp.Model.Dto;
using TweetApp.Repository.Entities;

namespace TweetApp.Service.Mapper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserDetailsDto, User>().ReverseMap();
            CreateMap<TweetCreateDto, Tweet>().ReverseMap();
            CreateMap<TweetDetailsDto, Tweet>().ReverseMap();
            CreateMap<ReplyTweetDto, ReplyTweet>().ReverseMap();
            CreateMap<ReactionResponse,Reaction>().ReverseMap();
            CreateMap<ReplyResponse,ReplyTweet>().ReverseMap();
        }
    }
}