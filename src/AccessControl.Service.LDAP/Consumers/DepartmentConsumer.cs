﻿using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Services;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    internal class DepartmentConsumer : IConsumer<IValidateDepartment>, IConsumer<IListDepartments>
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
            IEnumerable<IDepartment> departments;

            if (Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
                departments = _ldapService.FindDepartmentsByManager(context.UserName());
            else
                departments = Enumerable.Empty<IDepartment>();

            return context.RespondAsync(ListCommand.DepartmentsResult(departments.ToArray()));
        }

        /// <summary>
        ///     Validates the specified department.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IValidateDepartment> context)
        {
            var departmentExists = _ldapService.ValidateDepartment(context.Message.Site, context.Message.Department);
            return context.RespondAsync(departmentExists ? new VoidResult() : new VoidResult("Invalid site or department specified"));
        }
    }
}