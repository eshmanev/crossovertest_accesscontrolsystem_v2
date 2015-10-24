using System;

namespace AccessPointSimulator.Dialog
{
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Property)]
    public class HelpTextAttribute : Attribute
    {
        public HelpTextAttribute(string helpText)
        {
            HelpText = helpText;
        }

        public string HelpText { get; }
    }
}