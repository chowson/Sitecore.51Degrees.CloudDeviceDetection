using System;
using System.Collections.Generic;
using System.Web;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface IFiftyOneDegreesService
    {
        void SetBrowserCapabilities();

        DetectedDevice GetDetectedDevice(string userAgent = null);
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

        public void SetBrowserCapabilities()
        {
            if (_httpContextWrapper.Items.Contains("FiftyOneDegreesService.SetBrowserCapabilities"))
            {
                return;
            }

            var detectedDevice = GetDetectedDevice();

            if (detectedDevice != null)
            {
                var browserCapabilities = _httpContextWrapper.Request.Browser;

                foreach (var deviceProperty in detectedDevice.DeviceProperties)
                {
                    browserCapabilities.Capabilities[deviceProperty] = detectedDevice[deviceProperty];
                }

                browserCapabilities.Capabilities["isMobileDevice"] = IsMobileDevice(detectedDevice);
                browserCapabilities.Capabilities["isTabletDevice"] = IsTabletDevice(detectedDevice);
            }

            _httpContextWrapper.Items.Add("FiftyOneDegreesService.SetBrowserCapabilities", true);
        }

        public DetectedDevice GetDetectedDevice(string userAgent = null)
        {
            userAgent = userAgent ?? _httpContextWrapper.Request.UserAgent;
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
                    Diagnostics.Log.Error(string.Format("51Degrees lookup error for user agent '{0}'", userAgent), this);
                    Diagnostics.Log.Error(exception.Message, exception);
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

        private string IsMobileDevice(DetectedDevice detectedDevice)
        {
            if (detectedDevice != null)
            {
                return (detectedDevice.IsMobile && detectedDevice.DeviceType.Equals("SmartPhone")).ToString();
            }

            return false.ToString();
        }

        private bool IsTabletDevice(DetectedDevice detectedDevice)
        {
            if (detectedDevice != null)
            {
                return detectedDevice.IsMobile && detectedDevice.DeviceType.Equals("Tablet");
            }

            return false;
        }
    }
}
