using NUnit.Framework;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Tests.Services
{
	[TestFixture]
	public class BrowserCapabilitiesTypeServiceTests
	{
		[TestCase("ScreenPixelsHeight", "321")]
		[TestCase("ScreenPixelsWidth", "123")]
		[TestCase("PlatformVersion", "3.1")]
		[TestCase("BrowserVersion", "3.1")]
		[TestCase("HasTouchScreen", "true")]
		[TestCase("Html5", "true")]
		[TestCase("IsConsole", "true")]
		[TestCase("IsEReader", "true")]
		[TestCase("IsMediaHub", "true")]
		[TestCase("IsMobile", "true")]
		[TestCase("IsSmallScreen", "true")]
		[TestCase("IsSmartPhone", "true")]
		[TestCase("IsTablet", "true")]
		[TestCase("IsTv", "true")]
		[TestCase("Javascript", "true")]
		public void CheckValueTypeReturnsTrueWhenCalledWithKnownKeyAndValueType(string key, string value)
		{
			BrowserCapabilitiesTypeServiceTester.Where()
				.CapabilityKeyIs(key)
				.CapabilityValueIs(value)
				.WhenCheckValueTypeIsCalled()
				.ThenTheCapbilityValueWasValid();
		}

		[TestCase("ScreenPixelsHeight")]
		[TestCase("ScreenPixelsWidth")]
		[TestCase("PlatformVersion")]
		[TestCase("BrowserVersion")]
		[TestCase("HasTouchScreen")]
		[TestCase("Html5")]
		[TestCase("IsConsole")]
		[TestCase("IsEReader")]
		[TestCase("IsMediaHub")]
		[TestCase("IsMobile")]
		[TestCase("IsSmallScreen")]
		[TestCase("IsSmartPhone")]
		[TestCase("IsTablet")]
		[TestCase("IsTv")]
		[TestCase("Javascript")]
		public void CheckValueTypeReturnsFalseWhenCalledWithKnownKeyAndValueType(string key)
		{
			BrowserCapabilitiesTypeServiceTester.Where()
				.CapabilityKeyIs(key)
				.CapabilityValueIs("test")
				.WhenCheckValueTypeIsCalled()
				.ThenTheCapbilityValueWasInvalid();
		}
	}

	internal class BrowserCapabilitiesTypeServiceTester
	{
		private string _key;
		private string _value;
		private bool _result;

		public static BrowserCapabilitiesTypeServiceTester Where()
		{
			return new BrowserCapabilitiesTypeServiceTester();
		}

		public BrowserCapabilitiesTypeServiceTester CapabilityKeyIs(string key)
		{
			_key = key;
			return this;
		}

		public BrowserCapabilitiesTypeServiceTester CapabilityValueIs(string value)
		{
			_value = value;
			return this;
		}

		public BrowserCapabilitiesTypeServiceTester WhenCheckValueTypeIsCalled()
		{
			_result = new BrowserCapabilitiesTypeService().CheckValueType(_key, _value);
			return this;
		}

		public void ThenTheCapbilityValueWasValid()
		{
			Assert.That(_result, Is.True, "Capability Value was expected to be valid but service deemed it was not.");
		}

		public void ThenTheCapbilityValueWasInvalid()
		{
			Assert.That(_result, Is.False, "Capability Value was expected to be invalid but service deemed it was valid.");
		}
	}
}
