using System.Configuration;

namespace AccessControl.Service.Configuration
{
    /// <summary>
    ///     Represents a security configuration element.
    /// </summary>
    public class SecurityElement : ConfigurationElement, ISecurityConfig
    {
        /// <summary>
        ///     The secret
        /// </summary>
        [ConfigurationProperty("secret")]
        public string Secret => (string) base["secret"];
    }
}