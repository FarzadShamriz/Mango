using Mango.Services.EmailAPI.Models.DTOs;

namespace Mango.Services.EmailAPI.Services
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task EmailUserWelcomeAndLog(string email);
    }
}
