using Sitecore.Data.Items;
using Sitecore.Rules;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Sitecore.Pipelines.HttpRequestBegin.DeviceDetection
{
    public class DeviceConditionProcessor : ResolveMobileDeviceProcessor
    {
        public override void Process(ResolveMobileDevicePipelineArgs args)
        {
            var matchingDevice = ExecuteDeviceConditions();

            if (matchingDevice != null)
            {
                args.DeviceId = matchingDevice.ID.ToString();
            }
        }

        private static DeviceItem ExecuteDeviceConditions()
        {
            var devices = Context.Database.Resources.Devices.GetAll();

            foreach (var device in devices)
            {
                var ruleContext = new RuleContext();

                foreach (var rule in RuleFactory.GetRules<RuleContext>(new[] { device.InnerItem }, "Rule").Rules)
                {
                    if (rule.Condition != null)
                    {
                        var stack = new RuleStack();
                        rule.Condition.Evaluate(ruleContext, stack);
                        if (ruleContext.IsAborted)
                        {
                            continue;
                        }
                        if ((stack.Count != 0) && ((bool)stack.Pop()))
                        {
                            return device;
                        }
                    }
                }
            }

            return null;
        }
    }
}
