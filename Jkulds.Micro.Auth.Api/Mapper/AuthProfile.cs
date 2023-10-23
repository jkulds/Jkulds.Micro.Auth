using AutoMapper;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Request;
using Jkulds.Micro.Auth.Api.Controllers.Models.Api.Response;
using Jkulds.Micro.Auth.Business.Services.AuthService.Dto;
using Jkulds.Micro.Options.Base;

namespace Jkulds.Micro.Auth.Api.Mapper;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<TokenResponse, JwtTokenDto>().ReverseMap();
        CreateMap<RefreshTokenRequest, JwtTokenDto>().ReverseMap();
        CreateMap<UserRegistrationRequest, UserRegistrationDto>().ReverseMap();
        CreateMap<UserLoginRequest, UserLoginDto>().ReverseMap();
        CreateMap<TokenResponse, BaseOption>().ReverseMap();
        CreateMap<UserUpdatePasswordRequest, UserUpdatePasswordDto>().ReverseMap();
    }
}