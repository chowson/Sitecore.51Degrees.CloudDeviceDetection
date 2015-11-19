using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface IBrowserCapabilitiesService
    {
        bool IsMobileDevice { get; }

        bool IsTabletDevice { get; }

        bool GetBoolProperty(string propertyName);

        decimal GetDecimalProperty(string propertyName, decimal defaultValue);

        int GetIntegerProperty(string propertyName, int defaultValue);

        string GetStringProperty(string propertyName);
    }

    public class BrowserCapabilitiesService : IBrowserCapabilitiesService
    {
        private readonly IHttpRequestWrapper _httpRequestWrapper;

        public BrowserCapabilitiesService(IHttpRequestWrapper httpRequestWrapper)
        {
            _httpRequestWrapper = httpRequestWrapper;
        }

        public bool IsMobileDevice
        {
            get { return _httpRequestWrapper.Browser.IsMobileDevice; }
        }

        public bool IsTabletDevice
        {
            get { return GetBoolProperty("isTabletDevice"); }
        }

        public bool GetBoolProperty(string propertyName)
        {
            var propertyValue = GetStringProperty(propertyName);

            if (!string.IsNullOrEmpty(propertyValue))
            {
                bool result;
                bool.TryParse(propertyValue, out result);
                return result;
            }

            return false;
        }

        public decimal GetDecimalProperty(string propertyName, decimal defaultValue)
        {
            var propertyValue = GetStringProperty(propertyName);

            if (!string.IsNullOrEmpty(propertyValue))
            {
                decimal result;
                decimal.TryParse(propertyValue, out result);
                return result;
            }

            return defaultValue;
        }

        public int GetIntegerProperty(string propertyName, int defaultValue)
        {
            var propertyValue = GetStringProperty(propertyName);

            if (!string.IsNullOrEmpty(propertyValue))
            {
                int result;
                int.TryParse(propertyValue, out result);
                return result;
            }

            return defaultValue;
        }

        public string GetStringProperty(string propertyName)
        {
            var browserCapabilities = _httpRequestWrapper.Browser.Capabilities;

            if (browserCapabilities.Contains(propertyName))
            {
                return browserCapabilities[propertyName].ToString();
            }

            return "";
        }
    }
}
