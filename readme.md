## What does it do?

This project provides device detection for Sitecore using the cloud web service provided by 51 Degrees (https://51degrees.com/products/device-detection/cloud).

It provides 4 things:

1. A _preprocessRequest_ pipeline step to populate the _Request.Browser_ with properties from 51degrees
2. An HttpRequestBegin pipeline step to run a condition set on the Sitecore Device item to determine context device. (not required for Sitecore 8 projects)
3. A set of rules that can be used for Conditional Renderings:
  * when the visitor's device is a:
    * Mobile
    * Tablet
    * Games Console
    * eReader
    * Media Hub
    * Small Screen
    * TV
  * when a visitor has a specific browser (e.g. Chrome)
  * when a visitor's browser version matches a condition
  * when a visitor is on a specific platform (e.g. iOS)
  * when a visitor is on a specific platform version
  * when a visitor's screen width pixels matches a condition
  * when a visitor's screen height pixels matches a condition
4. A 51Degrees implementation of _DeviceInformationProviderBase_ to enable built in Device Detection functionality within Sitecore 8 and Device Reporting within Experience Analytics of xDB
  
## What prerequisites do you need?

To use this module, you will need to sign up for an account with 51 degrees, all rules provided are available on their free tier.

For full Device Detection functionality in Sitecore 8, a _Premium Cloud_ subscription is required. If using the free subscription, the things that won't be reported are:
  * Platform Vendor
  * Hardware model
  * HasTouchScreen
  * Javascript

## Installation

1. Install the Sitecore package _/SitecorePackages/Sitecore.51Degrees.CloudDeviceDetection.zip_
  * If you are using Sitecore 8, click Skip on any conflicts (extra items are needed for compatibility with older Sitecore versions)
2. Do an Incremental Site Publish
3. Open _/App_Config/Include/Z.Sitecore.51Degrees.CloudDeviceDetection/Sitecore.51Degrees.CloudDeviceDetection.config_ and enter your license key in the setting _Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiLicenceKey_
4. If you are using Sitecore 8 and xDB, rename _/App_Config/Include/Z.Sitecore.51Degrees.CloudDeviceDetection/Sitecore.51Degrees.CloudDeviceDetection.xDB.config.bak to remove the .bak extension
  * If also using the free tier, remove the _Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiEndpoint_ setting from this file to revert back to the basic settings

To check that the plugin is working, navigate to _/Sitecore/Admin/DeviceDetectionStatus.aspx where you should see details about your device if the module is working.

## Customisations

A 51degrees endpoint is provided by default in the setting _Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiEndpoint_, you can override this but the module requires the values of _IsMobile_, _DeviceType_, _IsConsole_, _IsEReader_, _IsMediaHub_, _IsSmallScreen_, _IsSmartPhone_, _IsTablet_, _IsTv_, _BrowserName_, _BrowserVersion_, _ScreenPixelsHeight_, _ScreenPixelsWidth_, _PlatformName_ and _PlatformVersion_ to function.

The additional device resolver step is plumbed in after the the default Sitecore device resolver and will only run if the Sitecore device resolver has resolved the device to the default device. If the default device has been resolved, then the module will run the new pipeline _resolveMobileDevice_. This has been split into several steps to allow additional steps to be inserted into the pipeline to customise it's behaviour.

To extend the module further, you can add additional querystring parameters to the setting _Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiEndpoint_ based on the property dictionary (https://51degrees.com/resources/property-dictionary). Once this is done, the new properties can be accessed by calling the following code:

```
HttpContext.Current.Request.Browser["PROPERTYNAME"]
```

Or

```
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.Services;
using Sitecore.FiftyOneDegrees.CloudDeviceDetection.System.Wrappers;

...

IBrowserCapabilitiesService browserCapabilitiesService = new BrowserCapabilitiesService(new HttpContextWrapper().Request);
browserCapabilitiesService.GetStringProperty(propertyName);
browserCapabilitiesService.GetBoolProperty(propertyName);
browserCapabilitiesService.GetIntegerProperty(propertyName);
browserCapabilitiesService.GetDecimalProperty(propertyName);
```
