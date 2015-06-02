using System.Web;
using SystemDotNet = System;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IHttpContextWrapper
    {
        IHttpRequestWrapper Request { get; }

        HttpContextBase GetHttpContext();
    }

    public class HttpContextWrapper : IHttpContextWrapper
    {
        public IHttpRequestWrapper Request
        {
            get { return new HttpRequestWrapper(GetHttpContext().Request); }
        }

        public HttpContextBase GetHttpContext()
        {
            return new SystemDotNet.Web.HttpContextWrapper(HttpContext.Current);
        }
    }
}
