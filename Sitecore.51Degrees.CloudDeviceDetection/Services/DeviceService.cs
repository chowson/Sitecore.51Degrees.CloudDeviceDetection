using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface IDeviceService
    {
        string GetDeviceId();
    }

    public class DeviceService : IDeviceService
    {
        private readonly IBrowserCapabilitiesService _browserCapabilitiesService;
        private readonly IDeviceIds _deviceIds;

        public DeviceService(IBrowserCapabilitiesService browserCapabilitiesService, IDeviceIds deviceIds)
        {
            _browserCapabilitiesService = browserCapabilitiesService;
            _deviceIds = deviceIds;
        }

        public string GetDeviceId()
        {
            if (_browserCapabilitiesService.IsMobileDevice)
            {
                return _deviceIds.Mobile;
            }
            
            if (_browserCapabilitiesService.IsTabletDevice)
            {
                return _deviceIds.Tablet;
            }

            return _deviceIds.Default;
        }
    }
}
