using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class VisitorsDeviceType<T> : WhenCondition<T> where T : RuleContext
    {
        public string DeviceType { get; set; }

        protected override bool Execute(T ruleContext)
        {
            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();

            bool result;

            switch (DeviceTypeName)
            {
                case "Mobile":
                    result = fiftyOneDegreesService.IsMobileDevice();
                    break;
                case "Tablet":
                    result = fiftyOneDegreesService.IsTabletDevice();
                    break;
                default:
                    result = false;
                    break;
            }

            return result;
        }

        private string DeviceTypeName
        {
            get
            {
                Assert.IsNotNullOrEmpty(DeviceType, "DeviceType cannot be null");

                var deviceItem = Context.Database.GetItem(DeviceType);

                return deviceItem.Name;
            }
        }
    }
}
