using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class ScreenPixelsHeightCondition<T> : IntegerComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();

            var screenPixelsHeightString = fiftyOneDegreesService.GetStringProperty("ScreenPixelsHeight");
            int screenPixelsHeight = screenPixelsHeightString.Equals("Unknown")
                ? int.MaxValue
                : fiftyOneDegreesService.GetIntegerProperty("ScreenPixelsHeight", int.MaxValue);

            return Compare(screenPixelsHeight);
        }
    }
}
