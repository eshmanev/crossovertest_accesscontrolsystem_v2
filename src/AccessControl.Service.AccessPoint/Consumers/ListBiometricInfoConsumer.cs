using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using AccessControl.Service.Configuration;
using MassTransit;
using User = AccessControl.Data.Entities.User;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class ListBiometricInfoConsumer : IConsumer<IListUsersBiometric>
    {
        private readonly IRabbitMqConfig _config;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;
        private readonly IRepository<User> _userRepository;

        public ListBiometricInfoConsumer(IRabbitMqConfig config, IRequestClient<IListUsers, IListUsersResult> listUsersRequest, IRepository<User> userRepository)
        {
            Contract.Requires(config != null);
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(userRepository != null);

            _config = config;
            _listUsersRequest = listUsersRequest;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<IListUsersBiometric> context)
        {
            var requestResult = await _listUsersRequest.Request(ListCommand.Default);
            var users = requestResult.Users.ToList();
            var userNames = users.Select(x => x.UserName).ToList();
            var entities = _userRepository.Filter(x => userNames.Contains(x.UserName)).ToDictionary(x => x.UserName);
            var userBiometrics = users
                .Select(
                    x =>
                    {
                        User userEntity;
                        return new UserBiometric(x.Site, x.UserName)
                        {
                            Department = x.Department,
                            PhoneNumber = x.PhoneNumber,
                            DisplayName = x.DisplayName,
                            Email = x.Email,
                            BiometricHash = entities.TryGetValue(x.UserName, out userEntity) ? userEntity.BiometricHash : null
                        };
                    })
                .Cast<IUserBiometric>()
                .ToArray();
            await context.RespondAsync(ListCommand.Result(userBiometrics));
        }
    }
}