using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class ScreenPixelsHeightCondition<T> : IntegerComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var browserCapabilitiesService = new BrowserCapabilitiesService(new HttpContextWrapper().Request);

            var screenPixelsHeightString = browserCapabilitiesService.GetStringProperty("ScreenPixelsHeight");
            int screenPixelsHeight = screenPixelsHeightString.Equals("Unknown")
                ? int.MaxValue
                : browserCapabilitiesService.GetIntegerProperty("ScreenPixelsHeight", int.MaxValue);

            return Compare(screenPixelsHeight);
        }
    }
}
