using System;
using System.Linq;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data
{
    public interface IDeviceIds
    {
        string Default { get; }

        string Mobile { get; }

        string Tablet { get; }
    }

    public class DeviceIds : IDeviceIds
    {
        private readonly ISitecoreSettingsWrapper _sitecoreSettingsWrapper;

        public DeviceIds(ISitecoreSettingsWrapper sitecoreSettingsWrapper)
        {
            _sitecoreSettingsWrapper = sitecoreSettingsWrapper;
        }

        public string Default
        {
            get
            {
                return GetDeviceIdWithNameFallback("Sitecore.FiftyOneDegrees.CloudDeviceDetection.DefaultDeviceId", "Default");
            }
        }

        public string Mobile
        {
            get
            {
                return GetDeviceIdWithNameFallback("Sitecore.FiftyOneDegrees.CloudDeviceDetection.MobileDeviceId", "Mobile");
            }
        }

        public string Tablet
        {
            get
            {
                return GetDeviceIdWithNameFallback("Sitecore.FiftyOneDegrees.CloudDeviceDetection.TabletDeviceId", "Tablet");
            }
        }

        private string GetDeviceIdWithNameFallback(string settingName, string deviceName)
        {
            var defaultDeviceId = _sitecoreSettingsWrapper.GetSetting(settingName);

            if (string.IsNullOrEmpty(defaultDeviceId))
            {
                defaultDeviceId = GetDeviceIdByName(deviceName);
            }

            return defaultDeviceId;
        }

        private static string GetDeviceIdByName(string deviceName)
        {
            var item = Context.Database.GetItem(ItemIDs.DevicesRoot);

            var matchingDeviceItem =
                item.Children.FirstOrDefault(
                    device => device.Name.Equals(deviceName, StringComparison.OrdinalIgnoreCase));

            if (matchingDeviceItem != null)
            {
                return matchingDeviceItem.ID.ToString();
            }

            return "";
        }
    }
}
