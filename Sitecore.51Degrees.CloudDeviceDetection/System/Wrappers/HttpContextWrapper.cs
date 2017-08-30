using System.Collections;
using System.Web;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
	public interface IHttpContextWrapper
    {
        IHttpRequestWrapper Request { get; }

        HttpContext GetHttpContext();

        IDictionary Items { get; }
    }

    public class HttpContextWrapper : IHttpContextWrapper
    {
        private readonly HttpContext _httpContext;

        public HttpContextWrapper()
        {
            _httpContext = HttpContext.Current;
        }

        public HttpContextWrapper(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public IHttpRequestWrapper Request
        {
            get
			{
				if (GetHttpContext() == null || GetHttpContext().Request == null)
				{
					return null;
				}

				return new HttpRequestWrapper(GetHttpContext().Request);
			}
        }

        public HttpContext GetHttpContext()
        {
            return _httpContext;
        }

        public IDictionary Items
        {
            get { return _httpContext.Items; }
        }
    }
}
