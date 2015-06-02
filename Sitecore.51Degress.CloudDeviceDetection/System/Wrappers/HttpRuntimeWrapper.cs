using System.Web;
using System.Web.Caching;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IHttpRuntimeWrapper
    {
        Cache Cache { get; }
    }

    public class HttpRuntimeWrapper : IHttpRuntimeWrapper
    {
        public Cache Cache
        {
            get { return HttpRuntime.Cache; }
        }
    }
}
