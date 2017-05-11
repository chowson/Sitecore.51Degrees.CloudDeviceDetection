using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using System.Linq;

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
        public void SetBrowserCapabilitiesMarkedAsCalledPerRequest()
        {
            FiftyOneDegreesServiceTester.Where()
                .CallSetBrowserCapabilities()
                .VerifyMethodMarkedAsCalled();
        }

        [Test]
        public void ValidCapabilitiesAreAddedToBrowser()
        {
            FiftyOneDegreesServiceTester.Where()
                .SetupApiReponse()
                .CallSetBrowserCapabilities()
                .VerifyCapabilitiesAdded();
        }

		[Test]
		public void InvalidCapabilitiesAreNotAddedToBrowser()
		{
			FiftyOneDegreesServiceTester.Where()
				.SetupApiReponse()
				.SetCapabilityTypeCheckToReturnFalse()
				.CallSetBrowserCapabilities()
				.VerifyCapabilitiesAreNotAdded();
		}

		[Test]
        public void ApiCallIsNotMadeWhenCacheIsWarm()
        {
            FiftyOneDegreesServiceTester.Where()
                .CacheIsWarm()
                .CallGetDetectedDevice()
                .VerifyApiWasNotCalled();
        }

        [Test]
        public void DetectedDeviceIsCachedWhenRequested()
        {
            FiftyOneDegreesServiceTester.Where()
                .SetupApiReponse()
                .CallGetDetectedDevice()
                .VerifyApiResultWasCached();
        }

        internal class FiftyOneDegreesServiceTester
        {
            private object _apiResult = new object();
            private const string UserAgent = "UserAgent";
            private readonly Mock<IBrowserCapabilitiesTypeService> _browserCapabilitiesTypeService;
            private readonly Mock<IWebRequestWrapper> _webRequestWrapper;
            private readonly Mock<IHttpRuntimeCacheWrapper> _httpRuntimeCacheWrapper;
            private readonly Mock<IHttpContextWrapper> _httpContextWrapper;
            private readonly Mock<IHttpBrowserCapabilitiesWrapper> _httpBrowser;
            private readonly Mock<IDictionary> _httpContextItems;
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

                _httpContextItems = new Mock<IDictionary>();
                _httpContextWrapper = new Mock<IHttpContextWrapper>();
                _httpContextWrapper.Setup(x => x.Request).Returns(httpRequestWrapper.Object);
                _httpContextWrapper.Setup(x => x.Items).Returns(_httpContextItems.Object);
                _webRequestWrapper = new Mock<IWebRequestWrapper>();
				_browserCapabilitiesTypeService = new Mock<IBrowserCapabilitiesTypeService>();

				_browserCapabilitiesTypeService.Setup(x => x.CheckValueType(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

				_fiftyOneDegreesService = new FiftyOneDegreesService(sitecoreSettingsWrapper.Object, _httpContextWrapper.Object, _httpRuntimeCacheWrapper.Object, _webRequestWrapper.Object, _browserCapabilitiesTypeService.Object);
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

            public FiftyOneDegreesServiceTester CallGetDetectedDevice(string userAgent = null)
            {
                _fiftyOneDegreesService.GetDetectedDevice(userAgent);
                return this;
            }

            #region Setup Methods

            public FiftyOneDegreesServiceTester MarkSetBrowserCapabilitiesAsCalled()
            {
                _httpContextItems.Setup(x => x.Contains("FiftyOneDegreesService.SetBrowserCapabilities")).Returns(true);
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

	        public FiftyOneDegreesServiceTester SetCapabilityTypeCheckToReturnFalse()
	        {
				_browserCapabilitiesTypeService.Setup(x => x.CheckValueType(It.IsAny<string>(), It.IsAny<string>()));
				return this;
	        }

			#endregion

			#region Verify Methods

			public void VerifyMethodIsAborted()
            {
                _httpContextItems.Verify(x => x.Add("FiftyOneDegreesService.SetBrowserCapabilities", true), Times.Never);
            }

            public void VerifyMethodMarkedAsCalled()
            {
                _httpContextItems.Verify(x => x.Add("FiftyOneDegreesService.SetBrowserCapabilities", true), Times.Once);
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

			public void VerifyCapabilitiesAreNotAdded()
			{
				var result = (IDictionary<string, object>)_apiResult;
				var capabilities = (IDictionary<string, object>)result["Values"];

				foreach (var capability in capabilities.Keys)
				{
					Assert.That(_httpBrowser.Object.Capabilities.Keys.OfType<string>().Contains(capability), Is.False, "Invalid value for capability was addeed to Browser capabilities.");
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
