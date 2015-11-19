using Sitecore.Diagnostics;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules.DeviceDetection
{
    public class VisitorsDeviceType<T> : WhenCondition<T> where T : RuleContext
    {
        public string DeviceType { get; set; }

        protected override bool Execute(T ruleContext)
        {
            var httpRequest = new HttpContextWrapper().Request;
            var browserCapabilitiesService = new BrowserCapabilitiesService(new HttpContextWrapper().Request);

            bool result;

            switch (DeviceTypeName)
            {
                case "Mobile":
                    result = browserCapabilitiesService.IsMobileDevice;
                    break;
                case "Tablet":
                    result = browserCapabilitiesService.IsTabletDevice;
                    break;
                case "Console":
                    result = browserCapabilitiesService.GetBoolProperty("IsConsole");
                    break;
                case "eReader":
                    result = browserCapabilitiesService.GetBoolProperty("IsEReader");
                    break;
                case "Media Hub":
                    result = browserCapabilitiesService.GetBoolProperty("IsMediaHub");
                    break;
                case "Small Screen":
                    result = browserCapabilitiesService.GetBoolProperty("IsSmallScreen");
                    break;
                case "TV":
                    result = browserCapabilitiesService.GetBoolProperty("IsTV");
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
    }
}
