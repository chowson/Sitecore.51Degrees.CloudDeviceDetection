using System.Collections;
using System.Web;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IHttpBrowserCapabilitiesWrapper
    {
        IDictionary Capabilities { get; }

        bool IsMobileDevice { get; }
    }
    
    public class HttpBrowserCapabilitiesWrapper : IHttpBrowserCapabilitiesWrapper
    {
        private readonly HttpBrowserCapabilities _httpBrowserCapabilities;

        public HttpBrowserCapabilitiesWrapper(HttpBrowserCapabilities httpBrowserCapabilities)
        {
            _httpBrowserCapabilities = httpBrowserCapabilities;
        }

        public IDictionary Capabilities { get { return _httpBrowserCapabilities.Capabilities; } }

        public bool IsMobileDevice { get { return _httpBrowserCapabilities.IsMobileDevice; } }
    }
}
