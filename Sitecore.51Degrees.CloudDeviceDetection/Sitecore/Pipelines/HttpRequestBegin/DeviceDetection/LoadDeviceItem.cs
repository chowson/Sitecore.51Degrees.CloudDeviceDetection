using System;
using Sitecore.Data;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class LoadDeviceItem : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            if (!String.IsNullOrEmpty(args.DeviceId))
            {
                if (Context.Database != null)
                {
                    args.Device = Context.Database.GetItem(new ID(args.DeviceId));
                }
            }
        }
    }
}
