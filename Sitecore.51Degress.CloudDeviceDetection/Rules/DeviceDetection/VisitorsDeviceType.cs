using System.Web.UI;
using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class VisitorsDeviceType<T> : WhenCondition<T> where T : RuleContext
    {
        public string DeviceType { get; set; }

        private IFiftyOneDegreesService _fiftyOneDegreesService;

        protected override bool Execute(T ruleContext)
        {
            _fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create();

            bool result;

            switch (DeviceTypeName)
            {
                case "Mobile":
                    result = _fiftyOneDegreesService.IsMobileDevice();
                    break;
                case "Tablet":
                    result = _fiftyOneDegreesService.IsTabletDevice();
                    break;
                case "Console":
                    result = GetBoolProperty("IsConsole");
                    break;
                case "eReader":
                    result = GetBoolProperty("IsEReader");
                    break;
                case "Media Hub":
                    result = GetBoolProperty("IsMediaHub");
                    break;
                case "Small Screen":
                    result = GetBoolProperty("IsSmallScreen");
                    break;
                case "TV":
                    result = GetBoolProperty("IsTV");
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

                Assert.IsNotNull(deviceItem, "DeviceItem '{0}' cannot be found in context database", DeviceType);

                return deviceItem.Name;
            }
        }

        private bool GetBoolProperty(string propertyName)
        {
            var detectedDevice = _fiftyOneDegreesService.GetDetectedDevice();

            if (detectedDevice != null)
            {
                bool result;
                bool.TryParse(detectedDevice[propertyName], out result);
                return result;
            }

            return false;
        }
    }
}
