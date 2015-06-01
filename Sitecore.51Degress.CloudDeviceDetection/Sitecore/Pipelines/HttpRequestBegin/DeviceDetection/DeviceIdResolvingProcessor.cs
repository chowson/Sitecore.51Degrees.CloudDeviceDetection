using Sitecore.FiftyOneDegress.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegress.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
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
