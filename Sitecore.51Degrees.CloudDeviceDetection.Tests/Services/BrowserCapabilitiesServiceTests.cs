using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Services
{
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
