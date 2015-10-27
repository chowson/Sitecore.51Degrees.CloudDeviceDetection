using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.Rules;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class BrowserVersionCondition<T> : DecimalComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();

            var browserVersion = fiftyOneDegreesService.GetDecimalProperty("BrowserVersion", decimal.MinusOne);
            
            return Compare(browserVersion);
        }
    }
}
