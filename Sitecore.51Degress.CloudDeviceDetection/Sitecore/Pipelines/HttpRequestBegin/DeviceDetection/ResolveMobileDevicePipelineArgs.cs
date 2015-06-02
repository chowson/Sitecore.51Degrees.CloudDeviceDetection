using Sitecore.Data.Items;
using Sitecore.Pipelines;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class ResolveMobileDevicePipelineArgs : PipelineArgs
    {
        public string DeviceId { get; set; }

        public DeviceItem Device { get; set; }
    }
}
