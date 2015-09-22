using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
    public interface IDeviceService
    {
        string GetDeviceId();
    }

    public class DeviceService : IDeviceService
    {
        private readonly IFiftyOneDegreesService _fiftyOneDegreesService;
        private readonly IDeviceIds _deviceIds;

        public DeviceService(IFiftyOneDegreesService fiftyOneDegreesService, IDeviceIds deviceIds)
        {
            _fiftyOneDegreesService = fiftyOneDegreesService;
            _deviceIds = deviceIds;
        }

        public string GetDeviceId()
        {
            if (_fiftyOneDegreesService.IsMobileDevice())
            {
                return _deviceIds.Mobile;
            }
            
            if (_fiftyOneDegreesService.IsTabletDevice())
            {
                return _deviceIds.Tablet;
            }

            return _deviceIds.Default;
        }
    }
}
