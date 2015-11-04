using System.Diagnostics.Contracts;
using System.DirectoryServices;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Service.LDAP.Configuration;
using MassTransit;

namespace AccessControl.Service.LDAP.Consumers
{
    public class DepartmentConsumer : IConsumer<IValidateDepartment>, IConsumer<IListDepartments>
    {
        private readonly ILdapConfig _config;

        public DepartmentConsumer(ILdapConfig config)
        {
            Contract.Requires(config != null);
            _config = config;
        }

        public Task Consume(ConsumeContext<IListDepartments> context)
        {
            var directoryEntiry = new DirectoryEntry(_config.LdapPath, _config.UserName, _config.Password);
            var searcher = new DirectorySearcher(directoryEntiry) {Filter = "(objectClass=user)"};
            searcher.PropertiesToLoad.Add("department");

            var departments =
                from result in searcher.FindAll().Cast<SearchResult>()
                let siteDistinguished = result.GetDirectoryEntry().Parent.GetProperty("distinguishedName")
                let site = result.GetDirectoryEntry().Parent.GetProperty("name")
                let department = result.GetProperty("department")
                where !string.IsNullOrWhiteSpace(siteDistinguished) && !string.IsNullOrWhiteSpace(site) && !string.IsNullOrWhiteSpace(department)
                select new Department(siteDistinguished, site, department);

            return context.RespondAsync(new ListDepartmentsResult(departments.Cast<IDepartment>().Distinct().ToArray()));
        }

        public Task Consume(ConsumeContext<IValidateDepartment> context)
        {
            var path = _config.CombinePath(context.Message.Site);
            var directoryEntiry = new DirectoryEntry(path, _config.UserName, _config.Password);
            var searcher = new DirectorySearcher(directoryEntiry) {Filter = "(objectClass=user)"};
            searcher.PropertiesToLoad.Add("department");
            var departmentExists = searcher.FindAll().Cast<SearchResult>().Any(x => x.GetProperty("department") == context.Message.Department);
            return context.RespondAsync(departmentExists ? new VoidResult() : new VoidResult("Invalid site or department specified"));
        }
    }
}