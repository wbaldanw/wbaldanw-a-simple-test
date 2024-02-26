using Last.Simple.App.Domain.Contracts.Repositories;
using Last.Simple.App.Domain.Contracts.UseCases.Users;
using Last.Simple.App.Domain.CoreDomain;
using Last.Simple.App.Domain.Models.Users;

namespace Last.Simple.App.UseCases.Users
{
    public class CreateUserUC : ICreateUserUC
    {
        private readonly IUserRepository userRepository;

        public CreateUserUC(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<long> CreateUser(CreateUserRequest request)
        {
            var user = new User(request.UserName, request.Password);
            return await userRepository.AddUser(user);
        }
    }
}
