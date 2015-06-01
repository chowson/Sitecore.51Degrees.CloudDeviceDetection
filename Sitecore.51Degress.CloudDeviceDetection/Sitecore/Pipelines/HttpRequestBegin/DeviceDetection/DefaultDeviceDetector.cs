using Sitecore.FiftyOneDegress.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Settings;

namespace Sitecore.FiftyOneDegress.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
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
