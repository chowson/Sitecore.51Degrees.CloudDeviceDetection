using System.Web;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IHttpRequestWrapper
    {
        string UserAgent { get; }

        IHttpBrowserCapabilitiesWrapper Browser { get; }
    }

    public class HttpRequestWrapper : IHttpRequestWrapper
    {
        private readonly HttpRequest _request;

        public HttpRequestWrapper(HttpRequest request)
        {
            _request = request;
        }

        public string UserAgent
        {
            get { return _request.UserAgent; }
        }

        public IHttpBrowserCapabilitiesWrapper Browser
        {
            get { return new HttpBrowserCapabilitiesWrapper(_request.Browser); }
        }
    }
}
