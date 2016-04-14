using System.Collections.Generic;
using NUnit.Framework;
using Sitecore.CES.DeviceDetection;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services.Data;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Services
{
    [TestFixture]
    public class FiftyOneDegreesPropertyServiceTests
    {
        private FiftyOneDegreesDevicePropertyService _fiftyOneDegreesDevicePropertyService;

        [SetUp]
        public void Setup()
        {
            _fiftyOneDegreesDevicePropertyService = new FiftyOneDegreesDevicePropertyService();
        }

        [TestCase("SmartPhone", DeviceType.MobilePhone)]
        [TestCase("EReader", DeviceType.EReader)]
        [TestCase("Tablet", DeviceType.Tablet)]
        [TestCase("Tv", DeviceType.SettopBox)]
        [TestCase("MediaHub", DeviceType.MediaPlayer)]
        [TestCase("Desktop", DeviceType.Computer)]
        [TestCase("Non-Matching", DeviceType.Other)]
        public void ParseDeviceTypeReturnsCorrectType(string deviceTypeString, DeviceType expectedDeviceType)
        {
            var deviceType = _fiftyOneDegreesDevicePropertyService.ParseDeviceType(deviceTypeString);

            Assert.That(deviceType, Is.EqualTo(expectedDeviceType));
        }

        [TestCase("true", true)]
        [TestCase("false", false)]
        [TestCase("nonBool", false)]
        public void GetBooleanCapabilityReturnsSafeValue(string value, bool expectedValue)
        {
            var detectedDevice = CreateDetectedDevice("HasCamera", value);

            var safeBooleanValue = _fiftyOneDegreesDevicePropertyService.GetBooleanCapability(detectedDevice, "HasCamera");

            Assert.That(safeBooleanValue, Is.EqualTo(expectedValue));
        }

        [TestCase("23", 23)]
        [TestCase("nonInt", -1)]
        public void GetIntegerCapabilityReturnsSafeValue(string value, int expectedValue)
        {
            var detectedDevice = CreateDetectedDevice("ScreenWidth", value);

            var safeBooleanValue = _fiftyOneDegreesDevicePropertyService.GetIntegerCapability(detectedDevice, "ScreenWidth");

            Assert.That(safeBooleanValue, Is.EqualTo(expectedValue));
        }

        private DetectedDevice CreateDetectedDevice(string propertyKey, object propertyValue)
        {
            var properties = new Dictionary<string, object> { { propertyKey, new[] { propertyValue } } };

            return new DetectedDevice(properties);
        }
    }
}
