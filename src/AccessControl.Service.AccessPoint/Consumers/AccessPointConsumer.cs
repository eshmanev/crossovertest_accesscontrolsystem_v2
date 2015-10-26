﻿using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using MassTransit;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class AccessPointConsumer : IConsumer<IRegisterAccessPoint>, IConsumer<IListAccessPoints>
    {
        private readonly IRepository<Data.Entities.AccessPoint> _accessPointRepository;
        private readonly IRequestClient<IValidateDepartment, IVoidResult> _validateDepartmentRequest;

        public AccessPointConsumer(IRepository<Data.Entities.AccessPoint> accessPointRepository, IRequestClient<IValidateDepartment, IVoidResult> validateDepartmentRequest)
        {
            Contract.Requires(accessPointRepository != null);
            Contract.Requires(validateDepartmentRequest != null);

            _accessPointRepository = accessPointRepository;
            _validateDepartmentRequest = validateDepartmentRequest;
        }

        public async Task Consume(ConsumeContext<IRegisterAccessPoint> context)
        {
            var result = await _validateDepartmentRequest.Request(new ValidateDepartment(context.Message.AccessPoint.Site, context.Message.AccessPoint.Department));
            if (!result.Succeded)
            {
                context.Respond(result);
                return;
            }

            var accessPoint = new Data.Entities.AccessPoint
            {
                AccessPointId = context.Message.AccessPoint.AccessPointId,
                Name = context.Message.AccessPoint.Name,
                Description = context.Message.AccessPoint.Description,
                Site = context.Message.AccessPoint.Site,
                Department = context.Message.AccessPoint.Department
            };
            _accessPointRepository.Insert(accessPoint);
            context.Respond(new VoidResult());
        }

        public Task Consume(ConsumeContext<IListAccessPoints> context)
        {
            var entities = _accessPointRepository.Filter(x => x.Site == context.Message.Site && x.Department == context.Message.Department);
            var accessPoints =
                entities.Select(x => new Contracts.Helpers.AccessPoint(x.AccessPointId, x.Site, x.Department, x.Name) {Description = x.Description})
                        .Cast<IAccessPoint>()
                        .ToArray();

            return context.RespondAsync(new ListAccessPointsResult(accessPoints));
        }
    }
}