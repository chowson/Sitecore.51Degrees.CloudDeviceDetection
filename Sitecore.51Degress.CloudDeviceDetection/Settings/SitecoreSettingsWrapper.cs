namespace Sitecore.FiftyOneDegress.CloudDeviceDetection.Settings
{
    public interface ISitecoreSettingsWrapper
    {
        string GetSetting(string settingName);

        string GetSetting(string settingName, string defaultValue);
    }

    public class SitecoreSettingsWrapper : ISitecoreSettingsWrapper
    {
        public string GetSetting(string settingName)
        {
            return Configuration.Settings.GetSetting(settingName);
        }

        public string GetSetting(string settingName, string defaultValue)
        {
            return Configuration.Settings.GetSetting(settingName, defaultValue);
        }
    }
}
