using System;
using System.Collections.Generic;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services
{
	public interface IBrowserCapabilitiesTypeService
	{
		bool CheckValueType(string capabilityKey, string capabilityValue);
	}
	
	public class BrowserCapabilitiesTypeService : IBrowserCapabilitiesTypeService
	{
		private readonly IDictionary<string, Type> _browserCapabilityTypes = new Dictionary<string, Type>
		{
			{ "ScreenPixelsHeight", typeof(int) },
			{ "ScreenPixelsWidth", typeof(int) },
			{ "PlatformVersion", typeof(double) },
			{ "BrowserVersion", typeof(double) },
			{ "HasTouchScreen", typeof(bool) },
			{ "Html5", typeof(bool) },
			{ "IsConsole", typeof(bool) },
			{ "IsEReader", typeof(bool) },
			{ "IsMediaHub", typeof(bool) },
			{ "IsMobile", typeof(bool) },
			{ "IsSmallScreen", typeof(bool) },
			{ "IsSmartPhone", typeof(bool) },
			{ "IsTablet", typeof(bool) },
			{ "IsTv", typeof(bool) },
			{ "Javascript", typeof(bool) }
		};

		public bool CheckValueType(string capabilityKey, string capabilityValue)
		{
			if (_browserCapabilityTypes.ContainsKey(capabilityKey))
			{
				var type = _browserCapabilityTypes[capabilityKey];

				if (type == typeof(bool))
				{
					bool convertedBoolean;
					return bool.TryParse(capabilityValue, out convertedBoolean);
				}

				if (type == typeof(int))
				{
					int convertedInteger;
					return int.TryParse(capabilityValue, out convertedInteger);
				}

				if (type == typeof(double))
				{
					double convertedDouble;
					return double.TryParse(capabilityValue, out convertedDouble);
				}
			}

			return true;
		}
	}
}
