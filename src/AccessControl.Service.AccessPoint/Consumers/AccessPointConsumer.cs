using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Commands.Search;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Impl.Commands;
using AccessControl.Contracts.Impl.Dto;
using AccessControl.Data;
using AccessControl.Service.Security;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    /// <summary>
    ///     Represents a consumer of access points.
    /// </summary>
    public class AccessPointConsumer : IConsumer<IRegisterAccessPoint>,
                                       IConsumer<IUnregisterAccessPoint>,
                                       IConsumer<IListAccessPoints>,
                                       IConsumer<IFindAccessPointById>
    {
        private readonly IDatabaseContext _databaseContext;
        private readonly IRequestClient<IListDepartments, IListDepartmentsResult> _listDepartmentsRequest;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccessPointConsumer" /> class.
        /// </summary>
        /// <param name="databaseContext">The database context.</param>
        /// <param name="listDepartmentsRequest">The list departments request.</param>
        public AccessPointConsumer(IDatabaseContext databaseContext, IRequestClient<IListDepartments, IListDepartmentsResult> listDepartmentsRequest)
        {
            Contract.Requires(databaseContext != null);
            Contract.Requires(listDepartmentsRequest != null);

            _databaseContext = databaseContext;
            _listDepartmentsRequest = listDepartmentsRequest;
        }

        /// <summary>
        ///     Searches for an access point by its identifier.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task Consume(ConsumeContext<IFindAccessPointById> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager) &&
                !Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                return context.RespondAsync(new FindAccessPointByIdResult(null));
            }

            var accessPoint = _databaseContext.AccessPoints.GetById(context.Message.AccessPointId);
            return context.RespondAsync(new FindAccessPointByIdResult(accessPoint != null ? ConvertAccessPoint(accessPoint) : null));
        }

        /// <summary>
        ///     Returns a list of access points.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public Task Consume(ConsumeContext<IListAccessPoints> context)
        {
            var fetcher = RoleBasedDataFetcher.Create(
                _databaseContext.AccessPoints.GetAll,
                manager => _databaseContext.AccessPoints.Filter(x => x.ManagedBy == manager));

            var entities = fetcher.Execute();
            var accessPoints = entities.Select(ConvertAccessPoint).ToArray();
            return context.RespondAsync(ListCommand.AccessPointsResult(accessPoints));
        }

        /// <summary>
        ///     Registers a new access point.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IRegisterAccessPoint> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                context.Respond(new VoidResult("Not authorized"));
                return;
            }

            var result = await _listDepartmentsRequest.Request(ListCommand.WithoutParameters);
            var departmentExists = result.Departments.Any(x => x.DepartmentName == context.Message.AccessPoint.Department);
            if (!departmentExists)
            {
                context.Respond(result);
                return;
            }

            var accessPoint = new Data.Entities.AccessPoint
            {
                AccessPointId = context.Message.AccessPoint.AccessPointId,
                Name = context.Message.AccessPoint.Name,
                Description = context.Message.AccessPoint.Description,
                Department = context.Message.AccessPoint.Department,
                ManagedBy = Thread.CurrentPrincipal.UserName()
            };
            _databaseContext.AccessPoints.Insert(accessPoint);
            _databaseContext.Commit();
            context.Respond(new VoidResult());
        }

        /// <summary>
        ///     Unregister an access point.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Task Consume(ConsumeContext<IUnregisterAccessPoint> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                return context.RespondAsync(new VoidResult("Not authorized"));
            }

            var accessPoint = _databaseContext.AccessPoints.GetById(context.Message.AccessPointId);
            if (accessPoint != null)
            {
                _databaseContext.AccessPoints.Delete(accessPoint);
                _databaseContext.Commit();
            }
            return context.RespondAsync(new VoidResult());
        }

        private IAccessPoint ConvertAccessPoint(Data.Entities.AccessPoint entity)
        {
            return new Contracts.Impl.Dto.AccessPoint(entity.AccessPointId, entity.Department, entity.Name)
            {
                Description = entity.Description
            };
        }
    }
}