using System.Collections.Generic;

namespace AccessControl.Data.Entities
{
    /// <summary>
    ///     Represents a base class for access rights.
    /// </summary>
    public abstract class AccessRightsBase
    {
        private IList<AccessRuleBase> _accessRules = new List<AccessRuleBase>();

        /// <summary>
        ///     Gets a collection of the access rules.
        /// </summary>
        public virtual IEnumerable<AccessRuleBase> AccessRules
        {
            get { return _accessRules; }
            protected set { _accessRules = (IList<AccessRuleBase>) value; }
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        public virtual int Id { get; protected set; }

        /// <summary>
        ///     Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public abstract void Accept(IAccessRightsVisitor visitor);

        /// <summary>
        ///     Adds the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public virtual void AddAccessRule(AccessRuleBase rule)
        {
            if (!_accessRules.Contains(rule))
            {
                _accessRules.Add(rule);
                rule.AccessRights = this;
            }
        }

        /// <summary>
        ///     Removes the access rule.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public virtual void RemoveAccessRule(AccessRuleBase rule)
        {
            if (_accessRules.Contains(rule))
            {
                _accessRules.Remove(rule);
                rule.AccessRights = null;
            }
        }
    }
}