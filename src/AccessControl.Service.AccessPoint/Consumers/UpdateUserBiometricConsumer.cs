using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccessControl.Contracts;
using AccessControl.Contracts.Commands;
using AccessControl.Contracts.Helpers;
using AccessControl.Data;
using MassTransit;
using User = AccessControl.Data.Entities.User;

namespace AccessControl.Service.AccessPoint.Consumers
{
    public class UpdateUserBiometricConsumer : IConsumer<IUpdateUserBiometric>
    {
        private readonly IRequestClient<IFindUserByName, IFindUserByNameResult> _findUserRequest;
        private readonly IRepository<User> _userRepository;

        public UpdateUserBiometricConsumer(IRequestClient<IFindUserByName, IFindUserByNameResult> findUserRequest, IRepository<User> userRepository)
        {
            Contract.Requires(findUserRequest != null);
            Contract.Requires(userRepository != null);

            _findUserRequest = findUserRequest;
            _userRepository = userRepository;
        }

        public async Task Consume(ConsumeContext<IUpdateUserBiometric> context)
        {
            var user = await _findUserRequest.Request(new FindUserByName(context.Message.UserName));
            if (user == null)
            {
                context.Respond(new VoidResult("UserName", "User is not found"));
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