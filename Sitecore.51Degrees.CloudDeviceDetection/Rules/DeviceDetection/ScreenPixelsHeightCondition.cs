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

			IHttpRequestWrapper httpRequestWrapper = new HttpContextWrapper().Request;
			if (httpRequestWrapper == null)
			{
				return false;
			}

			var browserCapabilitiesService = new BrowserCapabilitiesService(httpRequestWrapper);

            var screenPixelsHeightString = browserCapabilitiesService.GetStringProperty("ScreenPixelsHeight");
            int screenPixelsHeight = screenPixelsHeightString.Equals("Unknown")
                ? int.MaxValue
                : browserCapabilitiesService.GetIntegerProperty("ScreenPixelsHeight", int.MaxValue);

            return Compare(screenPixelsHeight);
        }
    }
}
