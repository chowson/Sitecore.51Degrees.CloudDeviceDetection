using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.Rules;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
	public class BrowserVersionCondition<T> : DecimalComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

			IHttpRequestWrapper httpRequestWrapper = new HttpContextWrapper().Request;
			if(httpRequestWrapper == null)
			{
				return false;
			}

			var browserCapabilitiesService = new BrowserCapabilitiesService(httpRequestWrapper);

            var browserVersion = browserCapabilitiesService.GetDecimalProperty("BrowserVersion", decimal.MinusOne);
            
            return Compare(browserVersion);
        }
    }
}
