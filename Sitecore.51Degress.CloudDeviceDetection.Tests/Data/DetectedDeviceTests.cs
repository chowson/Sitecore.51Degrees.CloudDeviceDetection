using System.Collections.Generic;
using NUnit.Framework;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Data
{
    [TestFixture]
    public class DetectedDeviceTests
    {
        private IDictionary<string, object> _deviceProperties;

        [SetUp]
        public void Setup()
        {
            _deviceProperties = new Dictionary<string, object>();
        }

        [Test]
        public void IsMobileReturnsCorrectProperty()
        {
            _deviceProperties.Add("IsMobile", new[] {"True"});

            var detectedDevice = new DetectedDevice(_deviceProperties);

            Assert.IsTrue(detectedDevice.IsMobile);
        }

        [Test]
        public void DeviceTypeReturnsCorrectProperty()
        {
            _deviceProperties.Add("DeviceType", new[] {"SmartPhone"});

            var detectedDevice = new DetectedDevice(_deviceProperties);

            Assert.That(detectedDevice.DeviceType, Is.EqualTo("SmartPhone"));
        }

        [Test]
        public void IndexerPropertyReturnsCorrectProperty()
        {
            _deviceProperties.Add("Indexer", new[] {"Indexer"});

            var detectedDevice = new DetectedDevice(_deviceProperties);

            Assert.That(detectedDevice["Indexer"], Is.EqualTo("Indexer"));
        }

        [Test]
        public void GetPropertyReturnsCorrectProperty()
        {
            _deviceProperties.Add("GetProperty", new[] { "GetProperty" });

            var detectedDevice = new DetectedDevice(_deviceProperties);

            Assert.That(detectedDevice["GetProperty"], Is.EqualTo("GetProperty"));
        }

        [Test]
        public void MissingPropertyReturnsCorrectProperty()
        {
            _deviceProperties.Add("ValidProperty", new[] { "ValidProperty" });

            var detectedDevice = new DetectedDevice(_deviceProperties);

            Assert.IsNullOrEmpty(detectedDevice["MissingProperty"]);
        }
    }
}
