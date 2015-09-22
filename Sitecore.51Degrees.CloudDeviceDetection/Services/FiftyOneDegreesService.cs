using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface IFiftyOneDegreesService
    {
        bool IsMobileDevice();

        bool IsTabletDevice();

        DetectedDevice GetDetectedDevice();
    }

    public class FiftyOneDegreesService : IFiftyOneDegreesService
    {
        private readonly ISitecoreSettingsWrapper _sitecoreSettingsWrapper;
        private readonly IHttpContextWrapper _httpContextWrapper;
        private readonly IHttpRuntimeCacheWrapper _httpRuntimeCacheWrapper;
        private readonly IWebRequestWrapper _webRequestWrapper;

        public FiftyOneDegreesService(ISitecoreSettingsWrapper sitecoreSettingsWrapper,
            IHttpContextWrapper httpContextWrapper, IHttpRuntimeCacheWrapper httpRuntimeCacheWrapper,
            IWebRequestWrapper webRequestWrapper)
        {
            _sitecoreSettingsWrapper = sitecoreSettingsWrapper;
            _httpContextWrapper = httpContextWrapper;
            _httpRuntimeCacheWrapper = httpRuntimeCacheWrapper;
            _webRequestWrapper = webRequestWrapper;
        }

        public bool IsMobileDevice()
        {
            var detectedDevice = GetDetectedDevice();

            if (detectedDevice != null)
            {
                return detectedDevice.IsMobile && detectedDevice.DeviceType.Equals("SmartPhone");
            }

            return false;
        }

        public bool IsTabletDevice()
        {
            var detectedDevice = GetDetectedDevice();

            if (detectedDevice != null)
            {
                return detectedDevice.IsMobile && detectedDevice.DeviceType.Equals("Tablet");
            }

            return false;
        }

        public DetectedDevice GetDetectedDevice()
        {
            var userAgent = _httpContextWrapper.Request.UserAgent;

            var detectedDevice = _httpRuntimeCacheWrapper.Get<DetectedDevice>(CacheKey(userAgent));

            if (detectedDevice == null)
            {
                try
                {
                    var deviceDetectionResult = _webRequestWrapper.GetJson<dynamic>(ApiEndpointUrl(userAgent));
                    detectedDevice = ParseDeviceDetectionResult(deviceDetectionResult);

                    if (detectedDevice != null)
                    {
                        _httpRuntimeCacheWrapper.Set(CacheKey(userAgent), detectedDevice);
                    }
                }
                catch (Exception exception)
                {
                    Diagnostics.Log.Error(string.Format("51Degrees lookup error for user agent '{0}'", userAgent), exception);
                }
            }

            return detectedDevice;  
        }

        private static DetectedDevice ParseDeviceDetectionResult(dynamic deviceDetectionResult)
        {
            if (deviceDetectionResult != null && deviceDetectionResult["Values"] != null && (deviceDetectionResult["Values"] as IDictionary<string, object>) != null)
            {
                return new DetectedDevice(deviceDetectionResult["Values"] as IDictionary<string, object>);
            }

            return null;
        }

        private static string CacheKey(string userAgent)
        {
            return string.Format("Sitecore.FiftyOneDegrees.CloudDeviceDetection.FiftyOneDegreesService.IsMobileDevice({0})", userAgent);
        }

        private string ApiEndpointUrl(string userAgent)
        {
            var apiLicenceKey = _sitecoreSettingsWrapper.GetSetting("Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiLicenceKey");
            var apiEndpointUrl = _sitecoreSettingsWrapper.GetSetting("Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiEndpoint");

            return string.Format(apiEndpointUrl, apiLicenceKey, HttpUtility.UrlEncode(userAgent));
        }
    }
}
