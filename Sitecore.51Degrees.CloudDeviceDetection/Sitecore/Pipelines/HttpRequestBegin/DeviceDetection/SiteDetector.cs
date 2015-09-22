using System.Collections.Generic;
using System.Linq;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class SiteDetector : ResolveMobileDeviceProcessor
    {
        private readonly List<string> _ineligibleSites = new List<string>
        {
            "speak",
            "shell",
            "login",
            "admin",
            "service",
            "modules_shell",
            "modules_website",
            "scheduler",
            "system",
            "publisher"
        };

        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            if (_ineligibleSites.Any(s => s == Context.Site.Name))
            {
                args.AbortPipeline();
            }
        }
    }
}
