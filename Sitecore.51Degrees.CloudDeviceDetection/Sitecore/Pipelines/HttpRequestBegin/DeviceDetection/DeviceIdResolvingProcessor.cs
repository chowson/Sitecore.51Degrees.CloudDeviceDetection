﻿using System;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
﻿using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    [Obsolete]
    public class DeviceIdResolvingProcessor : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            var browserCapabilitiesService = new BrowserCapabilitiesService(new HttpContextWrapper().Request);
            var deviceIds = new DeviceIds(new SitecoreSettingsWrapper());
            IDeviceService requestDeviceService = new DeviceService(browserCapabilitiesService, deviceIds);

            args.DeviceId = requestDeviceService.GetDeviceId();
        }
    }
}
