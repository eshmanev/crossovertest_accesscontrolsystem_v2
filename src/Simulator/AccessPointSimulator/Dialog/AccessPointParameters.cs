using System;

namespace AccessPointSimulator.Dialog
{
    public class AccessPointParameters
    {
        public AccessPointParameters()
        {
            Id = Guid.NewGuid();
        }

        [HelpText("Enter an identifier of the access point")]
        public Guid Id { get; set; }

        [HelpText("Enter a name of the access point")]
        public string Name { get; set; }

        [HelpText("Enter a description")]
        public string Description { get; set; }
    }
}