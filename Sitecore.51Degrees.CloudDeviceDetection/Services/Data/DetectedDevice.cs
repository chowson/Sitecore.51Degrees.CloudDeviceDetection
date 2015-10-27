using System.Collections.Generic;
using System.Linq;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data
{
    public class DetectedDevice
    {
        private readonly IDictionary<string, object> _deviceProperties;

        public DetectedDevice(IDictionary<string, object> deviceProperties)
        {
            _deviceProperties = deviceProperties;
        }

        public bool IsMobile
        {
            get
            {
                var isMobileString = GetProperty("IsMobile");
                
                bool isMobile;
                bool.TryParse(isMobileString, out isMobile);
                return isMobile;
            }
        }

        public string DeviceType
        {
            get { return GetProperty("DeviceType"); }
        }

        public bool HasProperty(string propertyName)
        {
            return _deviceProperties.ContainsKey(propertyName);
        }

        public string this[string propertyName]
        {
            get { return GetProperty(propertyName); }
        }

        private string GetProperty(string propertyName)
        {
            if (_deviceProperties != null && _deviceProperties.ContainsKey(propertyName) && _deviceProperties[propertyName] != null)
            {
                var propertyValues = _deviceProperties[propertyName] as object[];

                if (propertyValues != null && propertyValues.Any())
                {
                    return propertyValues[0].ToString();
                }
            }

            return "";
        }
    }
}
