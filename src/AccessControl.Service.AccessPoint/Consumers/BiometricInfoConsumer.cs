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
    /// <summary>
    ///     Represents a consumer of users' biometric information.
    /// </summary>
    public class BiometricInfoConsumer : IConsumer<IListUsersBiometric>, IConsumer<IUpdateUserBiometric>
    {
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;
        private readonly IBus _bus;
        private readonly IRequestClient<IListUsers, IListUsersResult> _listUsersRequest;
        private readonly IRepository<User> _userRepository;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BiometricInfoConsumer" /> class.
        /// </summary>
        /// <param name="bus">The bus.</param>
        /// <param name="listUsersRequest">The list users request.</param>
        /// <param name="findUserRequest">The find user request.</param>
        /// <param name="userRepository">The user repository.</param>
        public BiometricInfoConsumer(IBus bus,
                                     IRequestClient<IListUsers, IListUsersResult> listUsersRequest,
                                     IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest,
                                     IRepository<User> userRepository)
        {
            Contract.Requires(bus != null);
            Contract.Requires(listUsersRequest != null);
            Contract.Requires(userRepository != null);
            Contract.Requires(findUserRequest != null);

            _bus = bus;
            _listUsersRequest = listUsersRequest;
            _findUserRequest = findUserRequest;
            _userRepository = userRepository;
        }

        /// <summary>
        ///     Returns a list of users with biometric information.
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
        ///     Updates biometric information for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IUpdateUserBiometric> context)
        {
            var response = await _findUserRequest.Request(new FindUserByName(context.Message.UserName));
            if (response.User == null)
            {
                await context.RespondAsync(new VoidResult("User is not found"));
                return;
            }

            string oldHash;
            var entity = _userRepository.Filter(x => x.UserName == context.Message.UserName).SingleOrDefault();
            if (entity != null)
            {
                oldHash = entity.BiometricHash;
                entity.BiometricHash = context.Message.BiometricHash;
                _userRepository.Update(entity);
            }
            else
            {
                oldHash = null;
                entity = new User {UserName = context.Message.UserName, BiometricHash = context.Message.BiometricHash};
                _userRepository.Insert(entity);
            }

            await _bus.Publish(Events.UserBiometricUpdated(entity.UserName, oldHash, entity.BiometricHash));
            await context.RespondAsync(new VoidResult());
        }
    }
}