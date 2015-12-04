using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Service.LDAP.Services;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    internal class DepartmentConsumer : IConsumer<IListDepartments>
    {
        private readonly ILdapService _ldapService;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DepartmentConsumer" /> class.
        /// </summary>
        /// <param name="ldapService">The LDAP service.</param>
        public DepartmentConsumer(ILdapService ldapService)
        {
            Contract.Requires(ldapService != null);
            _ldapService = ldapService;
        }

        /// <summary>
        ///     Returns a list of departments managed by the logged in user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListDepartments> context)
        {
            var fetcher = RoleBasedDataFetcher.Create(_ldapService.ListDepartments, _ldapService.FindDepartmentsByManager);
            var departments = fetcher.Execute();
            return context.RespondAsync(ListCommand.DepartmentsResult(departments.ToArray()));
        }
    }
}