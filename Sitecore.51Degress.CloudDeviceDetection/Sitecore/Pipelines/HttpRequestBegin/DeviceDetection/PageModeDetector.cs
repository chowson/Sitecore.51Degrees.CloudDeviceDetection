namespace Sitecore.FiftyOneDegress.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class PageModeDetector : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            if (!Context.PageMode.IsNormal)
            {
                args.AbortPipeline();
            }
        }
    }
}
