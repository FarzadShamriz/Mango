using Mongo.Web.Models;
using Mongo.Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mongo.Web.Services
{
    public class BaseService : IBaseService
    {
        private IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");
                //Token
                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;
                switch (requestDto.ApiType)
                {
                    case Utilities.SD.ApiType.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case Utilities.SD.ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case Utilities.SD.ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case Utilities.SD.ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);

                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        if(apiContent == null)
                        {
                            return new()
                            {
                                IsSuccess = false,
                                Message = "Not Found!",
                                Result = null
                            };
                        }
                        return JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Not Authorized!",
                            Result = null
                        };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Forbidden!",
                            Result = null
                        };
                    case System.Net.HttpStatusCode.NotFound:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Not Found!",
                            Result = null
                        };
                    case System.Net.HttpStatusCode.MethodNotAllowed:
                        return new()
                        {
                            IsSuccess = false,
                            Message = "Not Allowed!",
                            Result = null
                        };
                    default:
                        var apiContents = await apiResponse.Content.ReadAsStringAsync();
                        if (apiContents == null)
                        {
                            return new()
                            {
                                IsSuccess = false,
                                Message = "Not Found!",
                                Result = null
                            };
                        }
                        return JsonConvert.DeserializeObject<ResponseDto>(apiContents);
                }
            }
            catch (Exception ex)
            {
                return new()
                {
                    IsSuccess = false,
                    Message = ex.Message.ToString()
                };
            }

        }
    }
}
