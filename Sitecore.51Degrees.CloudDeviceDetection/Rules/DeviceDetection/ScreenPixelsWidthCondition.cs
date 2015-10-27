using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class ScreenPixelsWidthCondition<T> : IntegerComparisonCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, "ruleContext");

            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();

            var screenPixelsWidthString = fiftyOneDegreesService.GetStringProperty("ScreenPixelsWidth");
            int screenPixelsWidth = screenPixelsWidthString.Equals("Unknown")
                ? int.MaxValue
                : fiftyOneDegreesService.GetIntegerProperty("ScreenPixelsWidth", int.MaxValue);

            return Compare(screenPixelsWidth);
        }
    }
}
