using Sitecore.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.Pipelines;
using Sitecore.Pipelines.HttpRequest;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
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
