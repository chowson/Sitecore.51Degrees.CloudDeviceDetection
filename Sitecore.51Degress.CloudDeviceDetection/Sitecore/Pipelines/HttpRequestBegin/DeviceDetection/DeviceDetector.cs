using Sitecore.Data;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Settings;
using Sitecore.Pipelines;
using Sitecore.Pipelines.HttpRequest;

namespace Sitecore.FiftyOneDegress.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class DeviceDetector : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            var resolveMobilePipelineArgs = new ResolveMobileDevicePipelineArgs();
            IDeviceIds deviceIds = new DeviceIds(new SitecoreSettingsWrapper());

            CorePipeline.Run("resolveMobileDevice", resolveMobilePipelineArgs);

            if (resolveMobilePipelineArgs.Device != null && resolveMobilePipelineArgs.Device.ID != new ID(deviceIds.Default))
            {
                Context.Device = resolveMobilePipelineArgs.Device;
            }
        }
    }
}
