using Mango.Web.Models;
using Mango.Web.Services.IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Mango.Web.Utilities.SD;

namespace Mango.Web.Services
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();
                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    message.Headers.Add("Accept", "*/*");
                }
                else
                {
                    message.Headers.Add("Accept", "application/json");
                }

                //Token
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.ContentType == ContentType.MultipartFormData)
                {
                    var content = new MultipartFormDataContent();
                    foreach (var prop in requestDto.Data.GetType().GetProperties())
                    {
                        var propValue = prop.GetValue(requestDto.Data);
                        if (propValue != null && propValue is FormFile)
                        {
                            var file = (FormFile)propValue;
                            if (file != null)
                            {
                                content.Add(new StreamContent(file.OpenReadStream()), prop.Name, file.FileName);
                            }
                        }
                        else
                        {
                            content.Add(new StringContent(propValue == null ? "" : propValue.ToString()), prop.Name);
                        }
                    }
                    message.Content = content;
                }
                else
                {
                    if (requestDto.Data != null)
                    {
                        message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                    }
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
                        if (apiContent == null)
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
