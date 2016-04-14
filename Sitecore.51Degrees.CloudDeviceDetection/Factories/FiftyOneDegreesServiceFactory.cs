using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using HttpContextWrapper = Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers.HttpContextWrapper;

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
            return Create(new HttpContextWrapper());
        }

        public IFiftyOneDegreesService Create(IHttpContextWrapper httpContextWrapper)
        {
            return new FiftyOneDegreesService(new SitecoreSettingsWrapper(),
                httpContextWrapper, new HttpRuntimeCacheWrapper(httpContextWrapper, new HttpRuntimeWrapper()),
                new WebRequestWrapper(new JsonSerializer()));
        }
    }
}
