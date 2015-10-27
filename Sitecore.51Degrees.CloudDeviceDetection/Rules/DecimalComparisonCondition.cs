using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace Sitecore.FiftyOneDegrees.CloudDeviceDetection.Rules
{
    public abstract class DecimalComparisonCondition<T> : OperatorCondition<T> where T : RuleContext
    {
        public decimal Value { get; set; }

        protected bool Compare(decimal value)
        {
            switch (GetOperator())
            {
                case ConditionOperator.Equal:
                    return value == Value;
                case ConditionOperator.GreaterThanOrEqual:
                    return value >= Value;
                case ConditionOperator.GreaterThan:
                    return value > Value;
                case ConditionOperator.LessThanOrEqual:
                    return value <= Value;
                case ConditionOperator.LessThan:
                    return value < Value;
                case ConditionOperator.NotEqual:
                    return value != Value;
                default:
                    return false;
            }
        }
    }
}
