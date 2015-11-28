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
using AccessControl.Contracts.Impl.Events;
using AccessControl.Data;
using MassTransit;
using User = AccessControl.Data.Entities.User;

namespace AccessControl.Service.AccessPoint.Consumers
{
    /// <summary>
    ///     Represents a consumer of users' biometric information.
    /// </summary>
    public class BiometricInfoConsumer : IConsumer<IListUsersBiometric>, IConsumer<IUpdateUserBiometric>, IConsumer<IFindUserByBiometrics>
    {
        private readonly IBus _bus;
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;
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
        ///     Searches for a user by biometric hash.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IFindUserByBiometrics> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager) &&
                !Thread.CurrentPrincipal.IsInRole(WellKnownRoles.ClientService))
            {
                context.Respond(new FindUserByBiometricsResult(null));
                return;
            }

            var entity = _userRepository.Filter(x => x.BiometricHash == context.Message.BiometricHash).SingleOrDefault();
            if (entity == null)
            {
                context.Respond(new FindUserByBiometricsResult(null));
                return;
            }

            var findUserResult = await _findUserRequest.Request(new FindUserByName(entity.UserName));
            if (findUserResult.User == null)
            {
                context.Respond(new FindUserByBiometricsResult(null));
                return;
            }

            context.Respond(new FindUserByBiometricsResult(ConvertUser(findUserResult.User, entity.BiometricHash)));
        }

        /// <summary>
        ///     Returns a list of users with biometric information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IListUsersBiometric> context)
        {
            // response contains the filtered result
            var requestResult = await _listUsersRequest.Request(ListCommand.WithoutParameters);
            var users = requestResult.Users.ToList();
            var userNames = users.Select(x => x.UserName).ToList();
            var entities = _userRepository.Filter(x => userNames.Contains(x.UserName)).ToDictionary(x => x.UserName);
            var userBiometrics = users
                .Select(
                    x =>
                    {
                        User userEntity;
                        var hash = entities.TryGetValue(x.UserName, out userEntity) ? userEntity.BiometricHash : null;
                        return ConvertUser(x, hash);
                    })
                .ToArray();
            await context.RespondAsync(ListCommand.UsersBiometricResult(userBiometrics));
        }

        /// <summary>
        ///     Updates biometric information for the specified user.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public async Task Consume(ConsumeContext<IUpdateUserBiometric> context)
        {
            if (!Thread.CurrentPrincipal.IsInRole(WellKnownRoles.Manager))
            {
                await context.RespondAsync(new VoidResult("Not authorized"));
                return;
            }

            var response = await _findUserRequest.Request(new FindUserByName(context.Message.UserName));
            if (response.User == null)
            {
                await context.RespondAsync(new VoidResult("User is not found"));
                return;
            }

            var duplicated = _userRepository
                .Filter(x => x.BiometricHash == context.Message.BiometricHash && x.UserName != context.Message.UserName)
                .Any();

            if (duplicated)
            {
                await context.RespondAsync(new VoidResult("This biometric hash is reserved already"));
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

            await _bus.Publish(new UserBiometricUpdated(entity.UserName, oldHash, entity.BiometricHash));
            await context.RespondAsync(new VoidResult());
        }

        private IUserBiometric ConvertUser(IUser user, string biometricHash)
        {
            return new UserBiometric(user.Site, user.UserName, user.Groups)
            {
                Department = user.Department,
                PhoneNumber = user.PhoneNumber,
                DisplayName = user.DisplayName,
                Email = user.Email,
                BiometricHash = biometricHash,
                ManagerName = user.ManagerName
            };
        }
    }
}