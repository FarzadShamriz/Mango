using Mongo.Web.Models;

namespace Mongo.Web.Services.IServices
{
    public interface IBaseService
    {
        Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true);

    }
}
