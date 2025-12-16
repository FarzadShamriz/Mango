using Mango.Services.AuthAPI.Models.DTOs;

namespace Mango.Services.AuthAPI.Services.Iservice
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registrationRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
        Task<bool> AssignRole(string email,string roleName);
    }
}
