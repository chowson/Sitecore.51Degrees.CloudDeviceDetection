using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class PlatformNameCondition<T> : StringOperatorCondition<T> where T : RuleContext
    {
        public string PlatformName { get; set; }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();

            return Compare(fiftyOneDegreesService.GetStringProperty("PlatformName"), PlatformName);
        }
    }
}
