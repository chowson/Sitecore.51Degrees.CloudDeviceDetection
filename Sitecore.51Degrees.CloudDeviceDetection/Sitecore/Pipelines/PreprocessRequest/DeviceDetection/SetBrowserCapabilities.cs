using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Factories;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.Pipelines.PreprocessRequest;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.PreprocessRequest.DeviceDetection
{
    public class SetBrowserCapabilities : PreprocessRequestProcessor
    {
        public override void Process(PreprocessRequestArgs args)
        {
            var fiftyOneDegreesService = new FiftyOneDegreesServiceFactory().Create(new HttpContextWrapper(args.Context));
            fiftyOneDegreesService.SetBrowserCapabilities();
        }
    }
}
