using System;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    [Obsolete]
    public class DeviceIdResolvingProcessor : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();
            var deviceIds = new DeviceIds(new SitecoreSettingsWrapper());
            IDeviceService requestDeviceService = new DeviceService(fiftyOneDegreesService, deviceIds);

            args.DeviceId = requestDeviceService.GetDeviceId();
        }
    }
}
