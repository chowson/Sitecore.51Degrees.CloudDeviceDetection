using Moq;
using NUnit.Framework;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Services
{
    [TestFixture]
    public class DeviceServiceTests
    {
        [Test]
        public void WhenMobileDeviceResolvedMobileDeviceIdIsSet()
        {
            DeviceIdResolverServiceTester.Where()
                .CurrentDeviceIsMobile()
                .ThenResolverDeviceId("Mobile");

        }

        [Test]
        public void WhenDeviceResolvedIsTabletDeviceIdIsSet()
        {
            DeviceIdResolverServiceTester.Where()
                .CurrentDeviceIsTablet()
                .ThenResolverDeviceId("Tablet");
        }

        [Test]
        public void WhenDeviceResolvedIsNotMobileDefaultDeviceIdIsSet()
        {
            DeviceIdResolverServiceTester.Where()
                .CurrentDeviceIsDesktop()
                .ThenResolverDeviceId("Default");
        }
    }

    internal class DeviceIdResolverServiceTester
    {
        private readonly Mock<IBrowserCapabilitiesService> _browserCapabilitiesService;
        private readonly DeviceService _deviceIdResolverService;

        private DeviceIdResolverServiceTester()
        {
            _browserCapabilitiesService = new Mock<IBrowserCapabilitiesService>();

            var deviceIds = new Mock<IDeviceIds>();
            deviceIds.Setup(x => x.Default).Returns("Default");
            deviceIds.Setup(x => x.Mobile).Returns("Mobile");
            deviceIds.Setup(x => x.Tablet).Returns("Tablet");

            _deviceIdResolverService = new DeviceService(_browserCapabilitiesService.Object, deviceIds.Object);
        }

        public static DeviceIdResolverServiceTester Where()
        {
            return new DeviceIdResolverServiceTester();
        }

        internal DeviceIdResolverServiceTester CurrentDeviceIsMobile()
        {
            SetUpServiceToReturnMobile(true);
            return this;
        }

        internal DeviceIdResolverServiceTester CurrentDeviceIsTablet()
        {
            SetUpServiceToReturnMobile(false);
            _browserCapabilitiesService.Setup(x => x.IsTabletDevice).Returns(true);
            return this;
        }

        internal DeviceIdResolverServiceTester CurrentDeviceIsDesktop()
        {
            SetUpServiceToReturnMobile(false);
            return this;
        }

        private void SetUpServiceToReturnMobile(bool isMobileDevice)
        {
            _browserCapabilitiesService.Setup(x => x.IsMobileDevice).Returns(isMobileDevice);
        }

        internal void ThenResolverDeviceId(string expectedDeviceId)
        {
            Assert.That(_deviceIdResolverService.GetDeviceId(), Is.EqualTo(expectedDeviceId));
        }
    }
}
