using System;
using System.IO;
using System.Net;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers
{
    public interface IWebRequestWrapper
    {
        T GetJson<T>(string requestUrl);
    }

    public class WebRequestWrapper : IWebRequestWrapper
    {
        private readonly ISerializer _serializer;

        public WebRequestWrapper(ISerializer serializer)
        {
            _serializer = serializer;
        }

        public T GetJson<T>(string requestUrl)
        {
            var webRequestResponse = MakeWebRequest(requestUrl);

            return _serializer.Deserialize<T>(webRequestResponse);
        }

        private static string MakeWebRequest(string requestUrl)
        {
            try
            {
                var request = WebRequest.Create(requestUrl);
                request.Timeout = 2000;

                var response = request.GetResponse();

                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (var reader = new StreamReader(responseStream))
                        {
                            return reader.ReadToEnd();
                        }
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                throw new Exception(string.Format("WebRequestWrapper: Error calling web service '{0}'", requestUrl), e);
            }
        }
    }
}
