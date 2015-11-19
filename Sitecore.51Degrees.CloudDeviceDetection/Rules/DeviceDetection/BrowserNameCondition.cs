using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class BrowserNameCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string BrowserName { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var browserCapabilitiesService = new BrowserCapabilitiesService(new HttpContextWrapper().Request);

            return Compare(browserCapabilitiesService.GetStringProperty("BrowserName"), BrowserName);
        }
    }
}
