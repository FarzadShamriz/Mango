using Mango.Services.EmailAPI.Data;
using Mango.Services.EmailAPI.Models;
using Mango.Services.EmailAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        private DbContextOptions<AppDbContext> _dbOptions;

        public EmailService(DbContextOptions<AppDbContext> dbOptions)
        {
            this._dbOptions = dbOptions;
        }

        public async Task EmailCartAndLog(CartDto cartDto)
        {
            StringBuilder logMessage = new StringBuilder();

            logMessage.AppendLine("<br/>Cart Email Requested ");
            logMessage.AppendLine("<br/>Total " + cartDto.CartHeader.CartTotal);
            logMessage.Append("<br/>");
            logMessage.Append("<ul>");
            foreach (var item in cartDto.CartDetails)
            {
                logMessage.Append("<li>");
                logMessage.Append(item.Product.Name + " x " + item.Count);
                logMessage.Append("</li>");
            }
            logMessage.Append("</ul>");

            await LogAndEmail(logMessage.ToString(), cartDto.CartHeader.Email);
        }


        public async Task RegisterUserEmailAndLog(string email)
        {
            string message = "User Registeration Successful. <br/> Email : " + email;
            await LogAndEmail(message, "dotnetmastery@gmail.com");
        }

        private async Task<bool> LogAndEmail(string message, string email)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = email,
                    EmailSentOn = DateTime.Now,
                    Message = message
                };
                await using var _db = new AppDbContext(_dbOptions);
                await _db.EmailLoggers.AddAsync(emailLog);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
