using Mango.Web.Models;

namespace Mango.Web.Services.IServices
{
    public interface IAuthService
    {
        Task<ResponseDto?> Login(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> Register(RegistrationRequestDto registrationRequestDto);
        Task<ResponseDto?> AssignRole(RegistrationRequestDto registrationRequestDto);
    }
}
