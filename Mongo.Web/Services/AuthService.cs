using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Mongo.Web.Utilities;

namespace Mongo.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;

        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto?> AssignRole(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = $"{SD.AuthAPIBase}/api/auth/AssignRole",
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto
            },
            withBearer: false);
        }

        public async Task<ResponseDto?> Login(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = $"{SD.AuthAPIBase}/api/auth/Login",
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto
            },
            withBearer: false);
        }

        public async Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                Url = $"{SD.AuthAPIBase}/api/auth/register",
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto
            },
            withBearer: false);
        }
    }
}
