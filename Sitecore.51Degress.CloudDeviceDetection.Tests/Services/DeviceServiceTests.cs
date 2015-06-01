using Moq;
using NUnit.Framework;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Data;
using Sitecore.FiftyOneDegress.CloudDeviceDetection.Services;

namespace Sitecore.FiftyOneDegress.CloudDeviceDetection.Tests.Services
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
        private readonly Mock<IFiftyOneDegreesService> _fiftyOneDegreesService;
        private readonly DeviceService _deviceIdResolverService;

        private DeviceIdResolverServiceTester()
        {
            _fiftyOneDegreesService = new Mock<IFiftyOneDegreesService>();

            var deviceIds = new Mock<IDeviceIds>();
            deviceIds.Setup(x => x.Default).Returns("Default");
            deviceIds.Setup(x => x.Mobile).Returns("Mobile");
            deviceIds.Setup(x => x.Tablet).Returns("Tablet");

            _deviceIdResolverService = new DeviceService(_fiftyOneDegreesService.Object, deviceIds.Object);
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
            _fiftyOneDegreesService.Setup(x => x.IsTabletDevice()).Returns(true);
            return this;
        }

        internal DeviceIdResolverServiceTester CurrentDeviceIsDesktop()
        {
            SetUpServiceToReturnMobile(false);
            return this;
        }

        private void SetUpServiceToReturnMobile(bool isMobileDevice)
        {
            _fiftyOneDegreesService.Setup(x => x.IsMobileDevice()).Returns(isMobileDevice);
        }

        internal void ThenResolverDeviceId(string expectedDeviceId)
        {
            Assert.That(_deviceIdResolverService.GetDeviceId(), Is.EqualTo(expectedDeviceId));
        }
    }
}
