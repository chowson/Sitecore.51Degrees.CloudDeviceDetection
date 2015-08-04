using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories
{
    public interface IFiftyOneDegreesServiceFactory
    {
        IFiftyOneDegreesService Create();
    }

    public class FiftyOneDegreesServiceFactory : IFiftyOneDegreesServiceFactory
    {
        public IFiftyOneDegreesService Create()
        {
            return new FiftyOneDegreesService(new SitecoreSettingsWrapper(),
                new HttpContextWrapper(), new HttpRuntimeCacheWrapper(new HttpContextWrapper(), new HttpRuntimeWrapper()), new WebRequestWrapper(new JsonSerializer()));
        }
    }
}
