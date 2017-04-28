using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
//using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
//using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using System.Web;
using System.IO;
using System;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Settings;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Services
{
    public class FakeWebRequestWrapper : IWebRequestWrapper
    {
        public T GetJson<T>(string requestUrl)
        {
            var serialiser = new JsonSerializer();
            var device = serialiser.Deserialize<T>("{\"MatchMethod\":\"Exact\",\"Difference\":0,\"DetectionTime\":0.0,\"Values\":{\"BrowserName\":[\"Chrome\"],\"BrowserVersion\":[\"57\"],\"DeviceType\":[\"Desktop\"],\"IsConsole\":[\"False\"],\"IsEReader\":[\"False\"],\"IsMediaHub\":[\"False\"],\"IsMobile\":[\"False\"],\"IsSmallScreen\":[\"False\"],\"IsSmartPhone\":[\"False\"],\"IsTablet\":[\"False\"],\"IsTv\":[\"False\"],\"PlatformName\":[\"Windows\"],\"PlatformVersion\":[\"8.1\"],\"ScreenPixelsHeight\":[\"Unknown\"],\"ScreenPixelsWidth\":[\"Unknown\"]},\"DataSetName\":\"PremiumV3\",\"Published\":\"2017-04-19T00:00:00Z\",\"SignaturesCompared\":0,\"ProfileIds\":{\"1\":15364,\"2\":21460,\"3\":69850,\"4\":18092},\"Useragent\":\"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko  Chrome/57            Safari/537\",\"TargetUseragent\":\"Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36\"}");
            return device;
        }
    }

    [TestFixture]
    public class BrowserCapabilitiesServiceTests
    {
        [Test]
        public void IsMobileDeviceReturnsCorrectValue()
        {
            BrowserCapabilitiesServiceTester.Where()
                .SetupIsMobile()
                .VerifyIsMobile();
        }

        [Test]
        public void IsTabletDeviceReturnsCorrectValue()
        {
            BrowserCapabilitiesServiceTester.Where()
                .SetupCapability("isTabletDevice", "true")
                .VerifyBoolCapability("isTabletDevice", true);
        }

        [Test]
        public void GetBoolPropertyReturnsCorrectValue()
        {
            BrowserCapabilitiesServiceTester.Where()
                            .SetupCapability("boolProperty", "true")
                            .VerifyBoolCapability("boolProperty", true);
        }

        [Test]
        public void GetDecimalPropertyReturnsCorrectValue()
        {
            BrowserCapabilitiesServiceTester.Where()
                            .SetupCapability("decimalProperty", "0.22")
                            .VerifyDecimalCapability("decimalProperty", 0.22M);
        }

        [Test]
        public void GetIntegerPropertyReturnsCorrectValue()
        {
            BrowserCapabilitiesServiceTester.Where()
                            .SetupCapability("integerProperty", "99")
                            .VerifyIntegerCapability("integerProperty", 99);
        }

        [Test]
        public void GetStringPropertyReturnsCorrectValue()
        {
            BrowserCapabilitiesServiceTester.Where()
                            .SetupCapability("stringProperty", "stringProperty")
                            .VerifyStringCapability("stringProperty", "stringProperty");
        }


        [Test]
        public void CheckScreenPixelWidthProperty() {
            //51 degrees returns "Unknown" for the screen pixel width for desktop browsers.
            //When a call is made to HttpContext.Current.Request.Browser.ScreenPixelsWidth that value is returned as an int.
            //That then throws an incorrect format exception.
            //This test has been built so tes that our fix will work.
            var workerrequest = new Mock<HttpWorkerRequest>();
            workerrequest.Setup(w => w.GetKnownRequestHeader(39)).Returns("Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");

            var request = new HttpRequest("", "http://test.com", "");
            HttpContext.Current = new HttpContext(request, new HttpResponse(new StringWriter()));

            var type = request.GetType();
            var field = type.GetField("_wr", global::System.Reflection.BindingFlags.NonPublic | global::System.Reflection.BindingFlags.Instance);
            field.SetValue(request, workerrequest.Object);
            var capsdictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            var caps = new HttpBrowserCapabilities() { Capabilities = capsdictionary };
            HttpContext.Current.Request.Browser = caps;

            var setting = new Mock<ISitecoreSettingsWrapper>();

            setting.Setup(i => i.GetSetting(It.IsAny<string>())).Returns("teststring");

            var httpcontextWrapper = new FiftyOneDegrees.CloudDeviceDetection.System.Wrappers.HttpContextWrapper(HttpContext.Current);
            var httpruntimeCache = new Mock<IHttpRuntimeCacheWrapper>();

            var httpWebRequest = new FakeWebRequestWrapper();

            var sut = new FiftyOneDegreesService(setting.Object, httpcontextWrapper, httpruntimeCache.Object, httpWebRequest);

            sut.SetBrowserCapabilities();

            Assert.IsTrue(request.Browser.ScreenPixelsWidth > 0);
        }



        internal class BrowserCapabilitiesServiceTester
        {
            private readonly IBrowserCapabilitiesService _browserCapabilitiesService;
            private readonly Mock<IHttpBrowserCapabilitiesWrapper> _httpBrowserCapabilities;
            private readonly IDictionary _capabilities = new Dictionary<string, string>();

            private BrowserCapabilitiesServiceTester()
            {
                _httpBrowserCapabilities = new Mock<IHttpBrowserCapabilitiesWrapper>();
                _httpBrowserCapabilities.Setup(x => x.Capabilities).Returns(_capabilities);

                var httpRequest = new Mock<IHttpRequestWrapper>();
                httpRequest.Setup(x => x.Browser).Returns(_httpBrowserCapabilities.Object);

                _browserCapabilitiesService = new BrowserCapabilitiesService(httpRequest.Object);
            }

            public static BrowserCapabilitiesServiceTester Where()
            {
                return new BrowserCapabilitiesServiceTester();
            }

            public BrowserCapabilitiesServiceTester SetupIsMobile()
            {
                _httpBrowserCapabilities.Setup(x => x.IsMobileDevice).Returns(true);
                return this;
            }

            public void VerifyIsMobile()
            {
                Assert.IsTrue(_browserCapabilitiesService.IsMobileDevice);
            }

            public BrowserCapabilitiesServiceTester SetupCapability(string key, string value)
            {
                _capabilities[key] = value;
                return this;
            }

            public void VerifyBoolCapability(string key, bool expectedValue)
            {
                Assert.That(_browserCapabilitiesService.GetBoolProperty(key), Is.EqualTo(expectedValue));
            }

            public void VerifyDecimalCapability(string key, decimal expectedValue)
            {
                Assert.That(_browserCapabilitiesService.GetDecimalProperty(key, -99), Is.EqualTo(expectedValue));
            }

            public void VerifyStringCapability(string key, string expectedValue)
            {
                Assert.That(_browserCapabilitiesService.GetStringProperty(key), Is.EqualTo(expectedValue));
            }

            public void VerifyIntegerCapability(string key, int expectedValue)
            {
                Assert.That(_browserCapabilitiesService.GetIntegerProperty(key, -99), Is.EqualTo(expectedValue));
            }
        }
    }
}
