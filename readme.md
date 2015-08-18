##What does it do?

This project provides device detection for Sitecore using the cloud web service provided by 51 Degrees (https://51degrees.com/products/device-detection/cloud).

It provides 2 things:
1. An HttpRequestBegin pipeline step to match the requesting device and set the Sitecore device appropriately
2. A set of rules that can be used as Conditional Renderings

##What prerequisites do you need?

To use this module, you will need to sign up for an account with 51 degrees.

##Installation

1. Install the Sitecore package _/SitecorePackages/Sitecore.51Degrees.CloudDeviceDetection.zip_
2. Open _/App_Config/Include/Sitecore.51Degrees.CloudDeviceDetection.config_ and enter your license key in the setting _Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiLicenceKey_
3. Add Device Detection (_{36C2B7B6-6B46-4C41-9E78-230DA2C5EC90}_) to /sitecore/system/Settings/Rules/Conditional Renderings/Tags/Default
4. If you want the device detection HttpRequestBegin pipeline to run, then create additional Sitecore device items to represent mobile and tablets as required and update the _/App_Config/Include/Sitecore.51Degrees.CloudDeviceDetection.config_ Sitecore settings. If you do not want this running, remove the piepline step.


##Customisations

By default, the module will switch to a device by naming convention, so if a request is found to be a mobile or tablet the module will try and find a device named mobile or tablet. You can override this in the configuration file _Sitecore.51Degrees.CloudDeviceDetection.config_ by adding the IDs of the devices for each device type. These settings are:

* Sitecore.FiftyOneDegrees.CloudDeviceDetection.DefaultDeviceId
* Sitecore.FiftyOneDegrees.CloudDeviceDetection.MobileDeviceId
* Sitecore.FiftyOneDegrees.CloudDeviceDetection.TabletDeviceId

A 51degrees endpoint is provided by default in the setting _Sitecore.FiftyOneDegrees.CloudDeviceDetection.ApiEndpoint_, you can override this but the module requires the values of _IsMobile_ and _DeviceType_ to function.

The additional device resolver step is plumbed in after the the default Sitecore device resolver and will only run if the Sitecore device resolver has resolved the device to the default device. If the default device has been resolved, then the module will run the new pipeline _resolveMobileDevice_. This has been split into several steps to allow additional steps to be inserted into the pipeline to customise it's behaviour.
