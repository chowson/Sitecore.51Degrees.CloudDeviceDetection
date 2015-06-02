using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Services
{
    [TestFixture]
    public class FiftyOneDegreesServiceTests
    {
        [Test]
        public void WhenRequestComesFromAMobileDeviceIsMobileReturnsTrue()
        {
            FiftyOneDegreesServiceTester.Where()
                .CurrentDeviceIsMobile()
                .ThenResolvesIsMobileDevice(true);
        }

        [Test]
        public void WhenRequestComesFromADesktopDeviceIsMobileReturnsFalse()
        {
            FiftyOneDegreesServiceTester.Where()
                .CurrentDeviceIsDesktop()
                .ThenResolvesIsMobileDevice(false);
        }

        [Test]
        public void WhenRequestComesFromTabletDeviceIsMobileReturnsFalse()
        {
            FiftyOneDegreesServiceTester.Where()
                .CurrentDeviceIsTablet()
                .ThenResolvesIsMobileDevice(false);
        }

        [Test]
        public void WhenCacheIsWarmThenApiIsNotCalled()
        {
            FiftyOneDegreesServiceTester.Where()
                .CacheIsWarm()
                .WhenIsMobileDeviceIsCalled()
                .ApiWasNotCalled();
        }

        [Test]
        public void WhenCacheIsColdThenResultIsCached()
        {
            FiftyOneDegreesServiceTester.Where()
                .CurrentDeviceIsMobile()
                .WhenIsMobileDeviceIsCalled()
                .ApiResultWasCached();
        }
    }

    internal class FiftyOneDegreesServiceTester
    {
        private object _apiResult;
        private const string UserAgent = "UserAgent";
        private readonly Mock<IWebRequestWrapper> _webRequestWrapper;
        private readonly Mock<IHttpRuntimeCacheWrapper> _httpRuntimeCacheWrapper;
        private readonly FiftyOneDegreesService _fiftyOneDegreesService;
        private const string FiftyOneDegreesEndpoint = "http://endpoint/{0}/?useragent={1}";
        private const string LicenceKey = "LicenceKey";
        private const string CacheKey = "Sitecore.FiftyOneDegrees.CloudDeviceDetection.FiftyOneDegreesService.IsMobileDevice({0})";

        private FiftyOneDegreesServiceTester()
        {
            var sitecoreSettingsWrapper = new Mock<ISitecoreSettingsWrapper>();
            sitecoreSettingsWrapper.Setup(x => x.GetSetting("Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiLicenceKey")).Returns(LicenceKey);
            sitecoreSettingsWrapper.Setup(x => x.GetSetting("Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiEndpoint")).Returns(FiftyOneDegreesEndpoint);

            _httpRuntimeCacheWrapper = new Mock<IHttpRuntimeCacheWrapper>();
            var httpRequestWrapper = new Mock<IHttpRequestWrapper>();
            httpRequestWrapper.Setup(x => x.UserAgent).Returns(UserAgent);

            var httpContextWrapper = new Mock<IHttpContextWrapper>();
            httpContextWrapper.Setup(x => x.Request).Returns(httpRequestWrapper.Object);
            _webRequestWrapper = new Mock<IWebRequestWrapper>();

            _fiftyOneDegreesService = new FiftyOneDegreesService(sitecoreSettingsWrapper.Object, httpContextWrapper.Object, _httpRuntimeCacheWrapper.Object, _webRequestWrapper.Object);
        }

        public static FiftyOneDegreesServiceTester Where()
        {
            return new FiftyOneDegreesServiceTester();
        }

        internal FiftyOneDegreesServiceTester CurrentDeviceIsMobile()
        {
            SetUpHttpContextWrapperToReturnMobile("true", "SmartPhone");
            return this;
        }

        internal FiftyOneDegreesServiceTester CurrentDeviceIsTablet()
        {
            SetUpHttpContextWrapperToReturnMobile("true", "Tablet");
            return this;
        }

        internal FiftyOneDegreesServiceTester CurrentDeviceIsDesktop()
        {
            SetUpHttpContextWrapperToReturnMobile("false", "Desktop");
            return this;
        }

        private void SetUpHttpContextWrapperToReturnMobile(string isMobileDevice, string deviceName)
        {
            SetupApiResult(isMobileDevice, deviceName);

            _webRequestWrapper.Setup(x => x.GetJson<dynamic>(FormattedEndpointUrl)).Returns(_apiResult);
        }

        internal void ThenResolvesIsMobileDevice(bool isMobileDevice)
        {
            Assert.That(_fiftyOneDegreesService.IsMobileDevice(), Is.EqualTo(isMobileDevice));
        }

        internal FiftyOneDegreesServiceTester CacheIsWarm()
        {
            _httpRuntimeCacheWrapper.Setup(x => x.Get<DetectedDevice>(FormattedCacheKey)).Returns(new DetectedDevice());
            return this;
        }

        internal FiftyOneDegreesServiceTester WhenIsMobileDeviceIsCalled()
        {
            _fiftyOneDegreesService.IsMobileDevice();
            return this;
        }

        internal void ApiResultWasCached()
        {
            _httpRuntimeCacheWrapper.Verify(x => x.Set(FormattedCacheKey, It.IsAny<DetectedDevice>()), Times.Once());
        }

        internal void ApiWasNotCalled()
        {
            _webRequestWrapper.Verify(x => x.GetJson<dynamic>(FormattedEndpointUrl), Times.Never());
        }

        private static string FormattedEndpointUrl
        {
            get { return string.Format(FiftyOneDegreesEndpoint, LicenceKey, UserAgent); }
        }

        private static string FormattedCacheKey
        {
            get { return string.Format(CacheKey, UserAgent); }
        }

        private void SetupApiResult(string isMobileDevice, string deviceName)
        {
            var result = new Dictionary<string, object>
            {
                {
                    "Values", new Dictionary<string, object>
                    {
                        { "DeviceType", new[] { deviceName } },
                        { "IsMobile", new[] { isMobileDevice } }
                    }
                }
            };

            _apiResult = result;
        }
    }
}
