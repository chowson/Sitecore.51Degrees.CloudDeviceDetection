using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class DeviceIdResolvingProcessor : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            var fiftyOneDegreesService = new FiftyOneDegreesService(new SitecoreSettingsWrapper(),
                new HttpContextWrapper(), new HttpRuntimeCacheWrapper(new HttpContextWrapper(), new HttpRuntimeWrapper()), new WebRequestWrapper(new JsonSerializer()));
            var deviceIds = new DeviceIds(new SitecoreSettingsWrapper());
            IDeviceService requestDeviceService = new DeviceService(fiftyOneDegreesService, deviceIds);

            args.DeviceId = requestDeviceService.GetDeviceId();
        }
    }
}
