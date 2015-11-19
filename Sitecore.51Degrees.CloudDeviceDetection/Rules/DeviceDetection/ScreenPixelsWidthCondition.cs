using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class ScreenPixelsWidthCondition<T> : IntegerComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var browserCapabilitiesService = new BrowserCapabilitiesService(new HttpContextWrapper().Request);

            var screenPixelsWidthString = browserCapabilitiesService.GetStringProperty("ScreenPixelsWidth");
            var screenPixelsWidth = screenPixelsWidthString.Equals("Unknown")
                ? int.MaxValue
                : browserCapabilitiesService.GetIntegerProperty("ScreenPixelsWidth", int.MaxValue);

            return Compare(screenPixelsWidth);
        }
    }
}
