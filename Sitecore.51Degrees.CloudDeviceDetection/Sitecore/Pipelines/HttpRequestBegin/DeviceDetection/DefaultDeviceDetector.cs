using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class DefaultDeviceDetector : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            if (Context.Device != null)
            {
                var contextDeviceId = Context.Device.ID.ToString();
                IDeviceIds deviceIds = new DeviceIds(new SitecoreSettingsWrapper());

                if (!contextDeviceId.Equals(deviceIds.Default))
                {
                    args.AbortPipeline();
                }
            }
        }
    }
}
