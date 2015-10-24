using System;

namespace AccessPointSimulator.Dialog
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CommandOptionAttribute : HelpTextAttribute
    {
        public CommandOptionAttribute(string command, string helpText)
            : base(helpText)
        {
            Command = command;
        }

        public string Command { get; }
    }
}