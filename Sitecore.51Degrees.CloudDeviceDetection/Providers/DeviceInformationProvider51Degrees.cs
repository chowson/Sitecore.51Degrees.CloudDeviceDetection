using System.Threading;
using Sitecore.CES.DeviceDetection;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;
using Sitecore.Threading.Locks;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Providers
{
    public class DeviceInformationProvider51Degrees : DeviceInformationProviderBase
    {
        private readonly ReaderWriterLockSlim _readerWriterLockSlim = new ReaderWriterLockSlim();
        private readonly IFiftyOneDegreesService _detection;
        private readonly IDevicePropertyService _devicePropertyService;

        public DeviceInformationProvider51Degrees()
        {
            _detection = new FiftyOneDegreesServiceFactory().Create();
            _devicePropertyService = new FiftyOneDegreesDevicePropertyService();
        }

        public override bool IsEnabled
        {
            get
            {
                try
                {
                    GetDeviceInformation("");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public override DeviceInformation GetDeviceInformation(string userAgent)
        {
            Assert.ArgumentNotNull(userAgent, "userAgent");

            return base.GetDeviceInformation(userAgent);
        }

        protected override DeviceInformation DoGetDeviceInformation(string userAgent)
        {
            Assert.ArgumentNotNull(userAgent, "userAgent");
            DetectedDevice deviceInformation;

            using (new ReadScope(_readerWriterLockSlim))
            {
                deviceInformation = _detection.GetDetectedDevice(userAgent);
            }

            return new DeviceInformation
            {
                Browser = deviceInformation["BrowserName"],
                BrowserCanJavaScript = _devicePropertyService.GetBooleanCapability(deviceInformation, "Javascript"), //Enterprise
                BrowserHtml5AudioCanAudio = _devicePropertyService.GetBooleanCapability(deviceInformation, "Html5"),
                BrowserHtml5VideoCanVideo = _devicePropertyService.GetBooleanCapability(deviceInformation, "Html5"),
                CanTouchScreen = _devicePropertyService.GetBooleanCapability(deviceInformation, "HasTouchScreen"), //Enterprise
                DeviceIsSmartphone = _devicePropertyService.GetBooleanCapability(deviceInformation, "IsSmartPhone"),
                DeviceModelName = deviceInformation["HardwareModel"] ?? GetEnterpriseLicenceWarning("HardwareModel"), //Enterprise
                DeviceOperatingSystemModel = deviceInformation["PlatformName"],
                DeviceOperatingSystemVendor = deviceInformation["PlatformVendor"] ?? GetEnterpriseLicenceWarning("PlatformVendor"), //Enterprise
                DeviceType = _devicePropertyService.ParseDeviceType(deviceInformation["DeviceType"]),
                DeviceVendor = deviceInformation["HardwareVendor"] ?? GetEnterpriseLicenceWarning("HardwareVendor"), //Enterprise
                HardwareDisplayHeight = _devicePropertyService.GetIntegerCapability(deviceInformation, "ScreenPixelsHeight"),
                HardwareDisplayWidth = _devicePropertyService.GetIntegerCapability(deviceInformation, "ScreenPixelsWidth")
            };
        }

        protected override string DoGetExtendedProperty(string userAgent, string propertyName)
        {
            Assert.ArgumentNotNull(userAgent, "userAgent");
            Assert.ArgumentNotNullOrEmpty(propertyName, "propertyName");

            DetectedDevice deviceInformation;
            using (new ReadScope(_readerWriterLockSlim))
            {
                deviceInformation = _detection.GetDetectedDevice(userAgent);
            }

            if (deviceInformation != null)
            {
                var parameterValue = deviceInformation[propertyName.ToLowerInvariant()];

                return parameterValue;
            }

            return null;
        }
        
        private static string GetEnterpriseLicenceWarning(string deviceProperty)
        {
            return string.Format("{0} requires Enterprise licence", deviceProperty);
        }
    }
}
