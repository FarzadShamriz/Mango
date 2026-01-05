using System.Net.Mime;
using static Mango.Web.Utilities.SD;
using ContentType = Mango.Web.Utilities.SD.ContentType;

namespace Mango.Web.Models
{
    public class RequestDto
    {
        public ApiType ApiType { get; set; } = ApiType.GET;
        public string Url { get; set; }
        public Object Data { get; set; }
        public string AccessToken { get; set; }
        public ContentType ContentType { get; set; } = ContentType.ApplicationJson;
    }
}
