using Last.Simple.App.Domain.CoreDomain;

namespace Last.Simple.App.Domain.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<long> AddUser(User user);
        Task<User?> GetByUserName(string userName);
    }
}
