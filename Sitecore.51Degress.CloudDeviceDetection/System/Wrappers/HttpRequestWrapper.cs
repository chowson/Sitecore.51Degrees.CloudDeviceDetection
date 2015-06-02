using System.Web;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IHttpRequestWrapper
    {
        string UserAgent { get; }
    }

    public class HttpRequestWrapper : IHttpRequestWrapper
    {
        private readonly HttpRequestBase _request;

        public HttpRequestWrapper(HttpRequestBase request)
        {
            _request = request;
        }

        public string UserAgent
        {
            get { return _request.UserAgent; }
        }
    }
}
