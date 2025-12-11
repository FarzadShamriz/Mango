using static Mongo.Web.Utilities.SD;

namespace Mongo.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public Object Data { get; set; }
        public string AccessToken { get; set; }
    }
}
