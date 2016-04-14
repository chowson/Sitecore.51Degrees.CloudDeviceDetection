using Sitecore.CES.DeviceDetection;
using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface IDevicePropertyService
    {
        DeviceType ParseDeviceType(string deviceTypeString);

        bool GetBooleanCapability(DetectedDevice detectedDevice, string propertyName);

        int GetIntegerCapability(DetectedDevice detectedDevice, string propertyName);
    }

    public class FiftyOneDegreesDevicePropertyService : IDevicePropertyService
    {
        public DeviceType ParseDeviceType(string deviceTypeString)
        {
            Assert.ArgumentNotNull(deviceTypeString, "deviceTypeString");
            switch (deviceTypeString)
            {
                case "SmartPhone":
                    return DeviceType.MobilePhone;
                case "EReader":
                    return DeviceType.EReader;
                case "Tablet":
                    return DeviceType.Tablet;
                case "MediaHub":
                    return DeviceType.MediaPlayer;
                case "Tv":
                    return DeviceType.SettopBox;
                case "Desktop":
                    return DeviceType.Computer;
                default:
                    return DeviceType.Other;
            }
        }

        public bool GetBooleanCapability(DetectedDevice detectedDevice, string propertyName)
        {
            var propertyValue = detectedDevice[propertyName];

            if (!string.IsNullOrEmpty(propertyValue))
            {
                bool result;
                bool.TryParse(propertyValue, out result);
                return result;
            }

            return false;
        }

        public int GetIntegerCapability(DetectedDevice detectedDevice, string propertyName)
        {
            var propertyValue = detectedDevice[propertyName];

            if (!string.IsNullOrEmpty(propertyValue))
            {
                int result;
                if (int.TryParse(propertyValue, out result))
                {
                    return result;
                }
            }

            return -1;
        }
    }
}
