using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Commands.Lists;
using AccessControl.Contracts.Commands.Management;
using AccessControl.Contracts.Dto;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using MassTransit;
using User = AccessControl.Data.Entities.User;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class BiometricInfoConsumer : IConsumer<IListUsersBiometric>, IConsumer<IUpdateUserBiometric>
    {
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;
        private readonly IRepository<User> _userRepository;

        public BiometricInfoConsumer(IRequestClient<IListUsers, IListUsersResult> listUsersRequest,
                                     IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest,
                                     IRepository<User> userRepository)
        {
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(userRepository != null);
            Contract.Requires(findUserRequest != null);

            _listUsersRequest = listUsersRequest;
            _findUserRequest = findUserRequest;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Returns a list of users with biometric information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates biometric information for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IUpdateUserBiometric> context)
        {
            var response = await _findUserRequest.Request(new FindUserByName(context.Message.UserName));
            if (response.User == null)
            {
                context.Respond(new VoidResult("User is not found"));
                return;
            }

            var entity = _userRepository.Filter(x => x.UserName == context.Message.UserName).SingleOrDefault();
            if (entity != null)
            {
                entity.BiometricHash = context.Message.BiometricHash;
                _userRepository.Update(entity);
            }
            else
            {
                entity = new User {UserName = context.Message.UserName, BiometricHash = context.Message.BiometricHash};
                _userRepository.Insert(entity);
            }

            context.Respond(new VoidResult());
        }
    }
}