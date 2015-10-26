using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Impl;
using AccessControl.Data;
using MassTransit;
using User = AccessControl.Data.Entities.User;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class ListBiometricInfoConsumer : IConsumer<IListBiometricInfo>
    {
        private readonly IRequestClient<IFindUsersByDepartment, IUser[]> _findUsersRequest;
        private readonly IRepository<User> _userRepository;

        public ListBiometricInfoConsumer(IRequestClient<IFindUsersByDepartment, IUser[]> findUsersRequest, IRepository<User> userRepository)
        {
            Contract.Requires(findUsersRequest != null);
            Contract.Requires(userRepository != null);

            _findUsersRequest = findUsersRequest;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<IListBiometricInfo> context)
        {
            var requestResult = await _findUsersRequest.Request(new FindUsersByDepartment(context.Message.Site, context.Message.Department));
            var users = requestResult.ToList();
            var userNames = users.Select(x => x.UserName).ToList();
            var entities = _userRepository.Filter(x => userNames.Contains(x.UserName)).ToDictionary(x => x.UserName);
            var responseMessage = users.Select(
                x =>
                {
                    User userEntity;
                    return new UserWithBiometric(x.Site, x.UserName)
                    {
                        Department = x.Department,
                        PhoneNumber = x.PhoneNumber,
                        DisplayName = x.DisplayName,
                        Email = x.Email,
                        BiometricHash = entities.TryGetValue(x.UserName, out userEntity) ? userEntity.BiometricHash : null
                    };
                })
                .Cast<IUserWithBiometric>()
                .ToArray();
            await context.RespondAsync(responseMessage);
        }
    }
}