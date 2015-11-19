using System.Collections.Generic;
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
        public void SetBrowserCapabilitiesIsOnlyCalledOncePerRequest()
        {
            FiftyOneDegreesServiceTester.Where()
                .MarkSetBrowserCapabilitiesAsCalled()
                .CallSetBrowserCapabilities()
                .VerifyMethodIsAborted();
        }

        [Test]
        public void ApiCallIsNotMadeWhenCacheIsWarm()
        {
            FiftyOneDegreesServiceTester.Where()
                .CacheIsWarm()
                .CallSetBrowserCapabilities()
                .VerifyApiWasNotCalled();
        }

        [Test]
        public void DetectedDeviceIsCachedWhenRequested()
        {
            FiftyOneDegreesServiceTester.Where()
                .SetupApiReponse()
                .CallSetBrowserCapabilities()
                .VerifyApiResultWasCached();
        }

        [Test]
        public void CapabilitiesAreAddedToBrowser()
        {
            FiftyOneDegreesServiceTester.Where()
                .SetupApiReponse()
                .CallSetBrowserCapabilities()
                .VerifyCapabilitiesAdded();
        }

        internal class FiftyOneDegreesServiceTester
        {
            private object _apiResult = new object();
            private const string UserAgent = "UserAgent";
            private readonly Mock<IWebRequestWrapper> _webRequestWrapper;
            private readonly Mock<IHttpRuntimeCacheWrapper> _httpRuntimeCacheWrapper;
            private readonly Mock<IHttpContextWrapper> _httpContextWrapper;
            private readonly Mock<IHttpBrowserCapabilitiesWrapper> _httpBrowser;
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

                _httpBrowser = new Mock<IHttpBrowserCapabilitiesWrapper>();
                _httpBrowser.Setup(x => x.Capabilities).Returns(new Dictionary<string, object>());

                var httpRequestWrapper = new Mock<IHttpRequestWrapper>();
                httpRequestWrapper.Setup(x => x.UserAgent).Returns(UserAgent);
                httpRequestWrapper.Setup(x => x.Browser).Returns(_httpBrowser.Object);

                _httpContextWrapper = new Mock<IHttpContextWrapper>();
                _httpContextWrapper.Setup(x => x.Request).Returns(httpRequestWrapper.Object);
                _httpContextWrapper.Setup(x => x.Items).Returns(new Dictionary<string, object>());
                _webRequestWrapper = new Mock<IWebRequestWrapper>();

                _fiftyOneDegreesService = new FiftyOneDegreesService(sitecoreSettingsWrapper.Object, _httpContextWrapper.Object, _httpRuntimeCacheWrapper.Object, _webRequestWrapper.Object);
            }

            public static FiftyOneDegreesServiceTester Where()
            {
                return new FiftyOneDegreesServiceTester();
            }

            public FiftyOneDegreesServiceTester CallSetBrowserCapabilities()
            {
                _fiftyOneDegreesService.SetBrowserCapabilities();
                return this;
            }

            #region Setup Methods

            public FiftyOneDegreesServiceTester MarkSetBrowserCapabilitiesAsCalled()
            {
                _httpContextWrapper.Object.Items.Add("FiftyOneDegreesService.SetBrowserCapabilities", "true");
                return this;
            }

            public FiftyOneDegreesServiceTester CacheIsWarm()
            {
                _httpRuntimeCacheWrapper.Setup(x => x.Get<DetectedDevice>(FormattedCacheKey)).Returns(new DetectedDevice(new Dictionary<string, object>()));
                return this;
            }

            public FiftyOneDegreesServiceTester SetupApiReponse()
            {
                _apiResult = new Dictionary<string, object>
                {
                    {
                        "Values", new Dictionary<string, object>
                        {
                            {"DeviceType", new[] {"deviceName"}},
                            {"IsMobile", new[] {"isMobileDevice"}}
                        }
                    }
                };

                _webRequestWrapper.Setup(x => x.GetJson<dynamic>(FormattedEndpointUrl)).Returns(_apiResult);

                return this;
            }

            #endregion

            #region Verify Methods

            public void VerifyMethodIsAborted()
            {
                _httpContextWrapper.Verify(x => x.Items.Add("FiftyOneDegreesService.SetBrowserCapabilities", true), Times.Never);
            }

            public void VerifyApiWasNotCalled()
            {
                _webRequestWrapper.Verify(x => x.GetJson<DetectedDevice>(FormattedEndpointUrl), Times.Never);
            }

            public void VerifyApiResultWasCached()
            {
                _httpRuntimeCacheWrapper.Verify(x => x.Set(FormattedCacheKey, It.IsAny<DetectedDevice>()), Times.Once());
            }

            public void VerifyCapabilitiesAdded()
            {
                var result = (IDictionary<string, object>)_apiResult;
                var capabilities = (IDictionary<string, object>)result["Values"];

                foreach (var capability in capabilities.Keys)
                {
                    Assert.That(_httpBrowser.Object.Capabilities[capability], Is.EqualTo(((string[])capabilities[capability])[0]));
                }
            }

            #endregion
            
            private static string FormattedCacheKey
            {
                get { return string.Format(CacheKey, UserAgent); }
            }

            private static string FormattedEndpointUrl
            {
                get { return string.Format(FiftyOneDegreesEndpoint, LicenceKey, UserAgent); }
            }
        }
    }
}
