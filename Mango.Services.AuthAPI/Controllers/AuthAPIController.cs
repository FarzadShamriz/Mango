using Mango.MessageBus;
using Mango.Services.AuthAPI.Models.DTOs;
using Mango.Services.AuthAPI.RabbitMQSender;
using Mango.Services.AuthAPI.Services.Iservice;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDto _responseDto;
        private readonly IRabbitMQAuthMessageSender _rabbitMQAuthMessageSender;
        private readonly IConfiguration _configuration;
        private readonly string _queueName;

        public AuthAPIController(IAuthService authService, IRabbitMQAuthMessageSender rabbitMQAuthMessageSender, IConfiguration configuration)
        {
            _authService = authService;
            _responseDto = new();
            _rabbitMQAuthMessageSender = rabbitMQAuthMessageSender;
            _configuration = configuration;
            _queueName = _configuration["UserRegisterQueue"];
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequestDto model)
        {
            var resultMsg = await _authService.Register(model);
            if (!string.IsNullOrEmpty(resultMsg))
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = resultMsg;
                return BadRequest(_responseDto);
            }

            _rabbitMQAuthMessageSender.SendMessage(model.Email, _queueName);

            return Ok(_responseDto);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResult = await _authService.Login(model);

            if(loginResult.User == null)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Username or Password is incorrect!!!";
                return BadRequest(_responseDto);
            }
            _responseDto.Result = loginResult;
            return Ok(_responseDto);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRole = await _authService.AssignRole(model.Email,model.Role.ToUpper());

            if (!assignRole)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = "Error!!!";
                return BadRequest(_responseDto);
            }
            return Ok(_responseDto);
        }


    }
}
